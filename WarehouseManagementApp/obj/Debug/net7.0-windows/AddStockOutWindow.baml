   M S B A M L   `   `   `  ����  WarehouseManagementApp.    (WarehouseManagementApp.AddStockOutWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Add New Stock Out��5   	   $	��600��6#   $	��800��60    � WindowStartupLocation$ CenterOwner=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Partner:��6   $��0��6$   $��0��61   $��0,0,10,5q�6A   �� 5   
   -   5         ��Name$ PartnerComboBox��5      $��0��6,   $��1��69   $��0,0,0,5q�6I   �� 5   
   $��	Date Out:��6   $��1��6%   $��0��62   $��0,0,10,5q�6B   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ DateOutPicker��5      $��1��6,   $��1��69   $��0,0,0,5q�6I   �� 5   
   $��Note:��6   $��2��6!   $��0��6.   $��0,0,10,5q�6>   �� 5   
   -   5         ��Name$ NoteTextBox��5      $��2��6'   $��1��64   $��0,0,0,5q�6D   �� 5   
   $��Add Products:��6   $��3��6)   $��0��66   $��2��6F   $
��Bold"�6Z   $��0,10,0,5q�6l   � 5   
   $��4��6   $��0��6   $��2��6,    5       �� 5!      $��* �6#   �� 5"      $
��Auto �6#   �� 5#      $
��Auto �6#    5%      �� 5&      $
��Auto �6    �� 5'      $��* �6    .+�5*      �� -   5        $ ProductComboBox��5*      $��0��60   $��0,0,5,5q�6@   �� 5+      -   5        $ QuantityTextBox��5+      $��1��6/   $��50��6?   $��0,0,5,5q�6J   �� 5,      -   5        $��Add Product��5,      $��2��6+   $��0,0,0,5q�6T   &   System.Windows.Controls.DataGrid  5.      -   5          Name$ ProductsDataGrid��5.      $��1��61   $��3��6>      AutoGenerateColumns$ False��5/      	   CanUserAddRows$	 False��63   $��0,5,0,0q�6J   
   Columns
 50      0  *System.Windows.Controls.DataGridTextColumn  51      ,  &System.Windows.Controls.DataGridColumn   Header$ Product��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6[   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *Product.Name+6[     52      $ Quantity��6)   $
 Auto 6X    6;   �� *
Quantity+6X   4  .System.Windows.Controls.DataGridTemplateColumn  53      $
 Auto 6-      CellTemplate 54      �� 55      .R�56   "   �� -   5        $��Remove��56   )   6:   57      59      5;      5<      �� 5>   
   -	   5        $��Add Stock Out��5>      $��5��6)   $��1��66   $��Right=�6F   $	��100��5?      $��0,10,0,0q�6   6/   5@      5A      