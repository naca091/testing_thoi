   M S B A M L   `   `   `  ����  WarehouseManagementApp-    'WarehouseManagementApp.LotDetailsWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Lot Details��5   	   $	��450��6   $	��800��6*    � WindowStartupLocation$ CenterOwner=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   �� $��0��6   .+�5      �� -   5         ��Name$ LotCodeTextBlock��5      $
��Bold"�62   �� 5      -   5        $ DateInTextBlock��5      �� 5      -   5        $ PartnerTextBlock��5      �� 5      -   5        $ NoteTextBlock��5      5      �� 5   
   $��1��6   $��Lot Details:��6!   $
��Bold"�65   $��0,10,0,5q�6G   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ LotDetailsDataGrid��5       $��2��6      AutoGenerateColumns$ False��6<      
IsReadOnly$
 True��6X      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Product��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 6W   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 66   �� *Product.Name+6W     5      $ Quantity��6%   $
	 Auto 6T   
 67   �� *
Quantity+6T   5      5      5      