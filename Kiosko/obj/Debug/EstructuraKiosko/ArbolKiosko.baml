   M S B A M L   `   `   `  ����E  AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls  5      A =Kiosko, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:Kiosko.EstructuraKiosko.Behavior Kiosko.EstructuraKiosko.Behavior 5      )   #Kiosko.EstructuraKiosko.ArbolKiosko   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Bexample.clr-namespace:Kiosko.EstructuraKiosko.Behavior     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
  5	      ���#1ba1e2.��5      / 	 )Telerik.Windows.Controls.RadBusyIndicator  -   5           Name$  BusyIndicator��5   	   + 	 %Telerik.Windows.Controls.StyleManager   Theme- 	 'Telerik.Windows.Controls.ThemeConverter$ Windows8Touch 5   	      IsIndeterminate . .��5   
   � -   5         �Name$ 
LayoutRoot��5      $��8q�6#    � 	Resources 5         ProductTemplate+ 	 %Telerik.Windows.Controls.RadDockPanel   Dock8 	 2Telerik.Windows.Controls.Windows8ResourceExtension� 	 �Telerik.Windows.Controls.ThemeResourceExtension`1[[Telerik.Windows.Controls.Windows8ResourceKey, Telerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7]]   ResourceKey? 	 9Telerik.Windows.Controls.Windows8ResourceKeyTypeConverter  ObrasChildTemplate �� ItemsSource �� Orientation  CategoryTemplate	 �� ItemTemplate
 �� 
TargetType. 	 (Telerik.Windows.DragDrop.DragDropManager �� Property �� Value.	  (Telerik.Windows.Controls.RadTreeViewItem��%�  &	        &	 �    &	 �    7  '��     '	  e    �� 5&      .R�5'        $	��200��6+   $��2q�6:   .+�5(      �� $	 Top=�5)      ��5*        $ MarkerBrush ��5+      �� *Titulo+�� 5,      $
 Left=�5-      ��5.        $ AccentBrush ��5/      �� *Precio+�� 50      $��2,0,0,0q�51      $
 Left=�52      $
��Wrap=�55      ��53        $ StrongBrush ��54      �� *Description+55      56      57      �� 59       6F   �� *	ObraChild+.R�5:      �� $ 
Horizontal=�6!   .+�5;      �� $
��Wrap=�6<   ��6$   �� *Titulo+6<   �� 5<      $��2q�5=      $��Center=�5>      
���  ����5@      �� *Orden+5A      5B      �� 5D      8	    5G      �� *	ObraChild+.R�5H      �� $ 
Horizontal=�6!   .+�5I      �� $��32��5J      $��32��5K      $��3q�5L      $��Center=�5M      ��5N      �� *	ImagePath+�� 5O      $��2q�5P      $��Center=�5Q      ��5R        $ MarkerBrush ��5S      �� *Titulo+5U      5V      �� 5X      "
 ��6   .�5Y      ��  �@ AllowCapturedDrag . 6R   5Z      �� 5\      "
 	 6   .�5]      ��  �@	 
IsExpanded .  63   5^      5_       � ColumnDefinitions 5`      �� 5a      $��* �6#   �� 5b      $	��.4* �6#   .+�5e      �� -   5         ��Name$ wishlistView��5f      $��1��5g      $��5,80,0,0q�5h      $��5q�5i      >
  8Kiosko.EstructuraKiosko.Behavior.ListBoxDragDropBehavior 
  	IsEnabled .  J� 	AllowDrop .  �� DisplayMemberPath$ Titulo��5l      $��14(�5m       �� SelectionMode$ Single=�5o       5p        DraggedItemTemplate��%�  &	       �� 5q      .R�5r      �� .+�5s      �� $ 
Horizontal=�6)   .+�5t   "   �� $��	Dragging:��6,   �� 5u   "   $
��Bold"�6,   ��6>   �� *CurrentDraggedItem.Titulo+5v      �� 5w      $ 
Horizontal=�6)   .+�5x   "   �� $��45��5y   %   $
��Bold"�5z   %   ��5{   %   �� *CurrentDropPosition+�� 5|   "   
�������$	��, (��6>   �� 5}   "   ��6,   �� *CurrentDraggedOverItem.Titulo+�� 5~   "   
�������$��)��6>   5      5�      5�      5�      5�      *  $Telerik.Windows.Controls.RadTreeView  5�      -   5          Name$ allProductsView��5�      $��0��5�      $��2��5�      $��0��5�      $��	0,80,15,0q�5�      $��5q�5�      
�������$��16(�5�         IsDragDropEnabled . #��[ 5�         TextDropAfter$ Después de:��5�         TextDropBefore$ 	Antes de:��5�         
TextDropIn$ Como hijo de:��5�         UIControls.SearchTextBox  5�      -   5        $��2��5�      $	��729��5�      $��30��5�      $��	10,45,0,0q�5�      $
��Left=�5�      $	��Top=�5�         	LabelText$ Buscar por obra��5�       �� TextWrapping$
 Wrap=�5�      �� 5�      $��1��5�      $��
10,10,10,0q�5�      $��Stretch=�5�      $	��Top=�5�      $��14(�5�      $
��Bold"�5�      $5��/Obras de catálogo que no han sido clasificadas��5�       �� TextAlignment$ Justify=�5�      $
��Wrap=�5�      �� 5�      $��	10,12,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��14(�5�      $
��Bold"�5�      $,��&Estructura del kiosko de publicaciones��5�      $ Justify=�5�      $
��Wrap=�5�      5�      5�      5�      