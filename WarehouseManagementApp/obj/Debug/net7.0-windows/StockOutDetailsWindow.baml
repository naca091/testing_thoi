   M S B A M L   `   `   `  ����  WarehouseManagementApp2    ,WarehouseManagementApp.StockOutDetailsWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Stock Out Details��5   	   $	��450��6#   $	��800��60    � WindowStartupLocation$ CenterOwner=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   �� $��0��6   .+�5      �� -   5         ��Name$ StockOutIdTextBlock��5      $
��Bold"�65   $��0,0,0,5q�6G   �� 5      -   5        $ PartnerTextBlock��5      $��0,0,0,5q�62   �� 5      -   5        $ DateOutTextBlock��5      $��0,0,0,5q�62   �� 5      -   5        $ NoteTextBlock��5      $��0,0,0,10q�6/   5      �� 5   
   $��1��6   $��Stock Out Details:��6!   $
��Bold"�6;   $��0,10,0,5q�6M   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ StockOutDetailsDataGrid��5       $��2��6      AutoGenerateColumns$ False��6A      
IsReadOnly$
 True��6]      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Product��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 6W   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 66   �� *Product.Name+6W     5      $ Quantity��6%   $
	 Auto 6T   
 67   �� *
Quantity+6T     5      $ 
Product ID��6%   $
	 Auto 6W   
 69   �� *	ProductId+6W     5      $ Stock Out Detail ID��6%   $
	 Auto 6g   
 6B   �� *StockOutDetailId+6g   5      5      5      