    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    namespace WarehouseManagementApp
    {
        public class ChatConnection : IDisposable
        {
            private HubConnection _connection;
            private bool _isConnecting;
            private bool _disposed;
            private readonly string _hubUrl;
            private readonly ItemsControl _messagesList;
            private readonly Dispatcher _dispatcher;
            private readonly int _maxRetries = 3;

            public bool IsConnected => _connection?.State == HubConnectionState.Connected;

            public ChatConnection(string hubUrl, ItemsControl messagesList, Dispatcher dispatcher)
            {
                _hubUrl = hubUrl ?? throw new ArgumentNullException(nameof(hubUrl));
                _messagesList = messagesList ?? throw new ArgumentNullException(nameof(messagesList));
                _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            }

            private void LogToUI(string message)
            {
                _dispatcher.InvokeAsync(() =>
                {
                    _messagesList.Items.Add($"{DateTime.Now:HH:mm:ss} - {message}");
                });
            }

            private async Task<bool> TestServerConnection()
            {
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(_hubUrl);
                    LogToUI($"Server response: {response.StatusCode}");
                    return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest;
                }
                catch (HttpRequestException ex)
                {
                    LogToUI($"Server test failed: {ex.Message}");
                    return false;
                }
            }

            private void RegisterHandlers()
            {
                _connection.Closed += async (error) =>
                {
                    LogToUI($"Connection closed. {(error != null ? $"Error: {error.Message}" : "")}");
                    await Task.CompletedTask;
                };

                _connection.Reconnecting += error =>
                {
                    LogToUI($"Attempting to reconnect... {(error != null ? $"Error: {error.Message}" : "")}");
                    return Task.CompletedTask;
                };

                _connection.Reconnected += connectionId =>
                {
                    LogToUI($"Reconnected! (ID: {connectionId})");
                    return Task.CompletedTask;
                };

                _connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    _dispatcher.InvokeAsync(() =>
                    {
                        _messagesList.Items.Add($"{DateTime.Now:HH:mm:ss} - {user}: {message}");
                    });
                });
            }

            public async Task<bool> EnsureConnectedAsync()
            {
                if (IsConnected)
                    return true;

                if (_isConnecting)
                    return false;

                try
                {
                    _isConnecting = true;
                    LogToUI("Testing server connection...");

                    if (!await TestServerConnection())
                    {
                        LogToUI("Server is not accessible. Please check if the server is running.");
                        return false;
                    }

                    if (_connection == null)
                    {
                        LogToUI("Initializing connection...");
                        _connection = new HubConnectionBuilder()
                            .WithUrl(_hubUrl, options =>
                            {
                                options.HttpMessageHandlerFactory = handler =>
                                {
                                    if (handler is HttpClientHandler clientHandler)
                                    {
                                        clientHandler.ServerCertificateCustomValidationCallback =
                                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                                    }
                                    return handler;
                                };
                            })
                            .WithAutomaticReconnect(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5) })
                            .Build();

                        RegisterHandlers();
                    }

                    int retryCount = 0;
                    while (retryCount < _maxRetries)
                    {
                        try
                        {
                            LogToUI($"Attempting to connect (Attempt {retryCount + 1}/{_maxRetries})...");
                            await _connection.StartAsync();
                            LogToUI("Connected successfully!");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            retryCount++;
                            if (retryCount == _maxRetries)
                            {
                                LogToUI($"Failed to connect after {_maxRetries} attempts: {ex.Message}");
                                if (ex.InnerException != null)
                                {
                                    LogToUI($"Details: {ex.InnerException.Message}");
                                }
                                return false;
                            }
                            await Task.Delay(1000 * retryCount);
                        }
                    }
                    return false;
                }
                finally
                {
                    _isConnecting = false;
                }
            }

            public async Task<bool> SendMessageAsync(string user, string message)
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    LogToUI("Message cannot be empty.");
                    return false;
                }

                try
                {
                    if (!await EnsureConnectedAsync())
                    {
                        LogToUI("Cannot send message: Not connected to server.");
                        return false;
                    }

                    await _connection.InvokeAsync("SendMessage", user, message);
                    return true;
                }
                catch (Exception ex)
                {
                    LogToUI($"Failed to send message: {ex.Message}");

                    if (ex is HubException || ex is InvalidOperationException)
                    {
                        _connection = null; // Force reconnection next time
                    }

                    return false;
                }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing && _connection != null)
                    {
                        try
                        {
                            _connection.DisposeAsync().AsTask().Wait(TimeSpan.FromSeconds(5));
                            LogToUI("Connection disposed.");
                        }
                        catch (Exception ex)
                        {
                            LogToUI($"Error during disposal: {ex.Message}");
                        }
                        finally
                        {
                            _connection = null;
                        }
                    }
                    _disposed = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~ChatConnection()
            {
                Dispose(false);
            }
        }
    }