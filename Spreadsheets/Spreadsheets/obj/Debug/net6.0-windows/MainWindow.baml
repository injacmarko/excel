   M S B A M L   `   `   `  ????G  CSpreadsheets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:SpreadsheetsSpreadsheets  5   	       Spreadsheets.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Spreadsheets     5   	     ? Title$  Spreadsheet??5   	   $	??450??6   $	??800??6*   .??5	      ? .+?5
   
   &   System.Windows.Controls.DataGrid  -   5        $??dg??5
   #     CanUserAddRows$ False??6      CanUserDeleteRows$ False??6*      CanUserSortColumns$ False??6D   $??0,30,0,0q?6_      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ 75 6?   $
??Left=?6?   $	??Top=?6?      SelectionUnit$
 Cell=?6-     SelectionMode$ Extended=?6B  ??6?   ??  ?? Source$
 data??6[  ?? 5   
   -   5        $
??Save??5   [   $
??Left=?6   $	??Top=?6,   $??30??6D   $??30??6O   $
??Save??6g   $??10(?6v   6?   ?? 5   
   -   5        $
??Open??5   [   $
??Left=?6   $	??Top=?6,   $??30??6D   $??30??6O   $
??Open??6g   $??10(?6v   $??35,0,0,0q?6?   ?? 5   
   -   5         ??Name$ btUndo??5      $
??Undo??6!   $
??Left=?60   $??30??6K   $??30??6V   $??70,0,0,0q?6b   $??10(?6t   $	??Top=?6?   6?   ?? 5   
   -   5        $ btRedo??5      $
??Redo??6!   $
??Left=?60   $??30??6K   $??30??6V   $??	105,0,0,0q?6b   $??10(?6u   $	??Top=?6?   6?   5      5      