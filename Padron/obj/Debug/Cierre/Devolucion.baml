   M S B A M L   `   `   `  ����
  Padron    Padron.Cierre.Devolucion   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a	 ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation     	 
  5      -   5        $	��350��5      $	��510��5      $��Realizar devolución��5      )  #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5
      (  "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��45 �6   .+�5   
   � $��0��6   $��2��6   .+�5      �� -   5         ��Name$ 
GpbOficina��5      $	��140��5      $��10,147,10,0q�5      $��Stretch=�5      $	��Top=�5      $��Propiedad de la oficina��5      .��5      � .+�5      �� $��	10,10,0,0q�5      $
��Left=�5      $	��Top=�5      $��	Recibió:��5       �� 5!      -   5         ��Name$ TxtRecibeOficina��5"      $��40��5#      $��23��5$      $��	66,10,0,0q�5%      $
��Left=�5&      $	��Top=�5'      {�.  ��5)      �� *	Oficina+�� 5*      $��
132,10,0,0q�5+      $
��Left=�5,      $	��Top=�5-      $��	Devolvió��5.      �� 5/      -   5        $ TxtDevolvioOficina��50      $��40��51      $��23��52      $��
196,10,0,0q�53      $
��Left=�54      $	��Top=�55      {�.   �� TextWrapping$
 Wrap=�57      �� 58      $��	10,54,0,0q�59      $
��Left=�5:      $	��Top=�5;      $��Esta devolviendo:��5<      / 
 )Telerik.Windows.Controls.RadNumericUpDown  5>      -   5          Name$ NumDevOficina��5?      $��56��5@      $��
117,58,0,0q�5A      $
��Left=�5B      $	��Top=�5C         	IsInteger . +  %Telerik.Windows.Controls.RadRangeBase   Minimum$ 0Q�5E      	   Value$	 0X�5F      5H      5I      �� 5J      $��	10,72,0,0q�5K      $
��Left=�5L      $	��Top=�5M      $��Fecha de devolución:��5N      , 
 &Telerik.Windows.Controls.RadDatePicker  5O      -   5        
  Name$
 RdpFechaDevuelve��5P      $	��181��5Q      $��
10,103,0,0q�5R      $
��Left=�5S      $	��Top=�5T      �� 5U      $��
15,304,0,0q�5V      $
��Left=�5W      $	��Top=�5X      $��Observaciones:��5Y      �� 5Z      -   5        $ TxtObservaciones��5[      $��80��5\      $��10,335,10,0q�5]      $��Stretch=�5^      $	��Top=�5_      $
 Wrap=�5`      �� 5a      $��	15,10,0,0q�5b      $
��Left=�5c      $	��Top=�5d      $��Oficio de devolución:��5e      �� 5f      -   5        $ 	TxtOficio��5g      $	��305��5h      $��23��5i      $��	15,41,0,0q�5j      $
��Left=�5k      $	��Top=�5l      $
 Wrap=�5m      5p      �� 5q   
   -	   5         ��Name$ BtnCancelar��5r      $��1��5s      $��90��5t      $��	0,0,10,10q�5u      $��Right=�5v      $��Bottom=�5w      $��Cancelar��5y       �� IsCancel .    
BlueButton#��[  5{      �� 5|   
   -
   5        $ 
BtnGuardar��5}      $��1��5~      $��92��5      $��
0,0,105,10q�5�      $��Right=�5�      $��Bottom=�5�      $��Guardar��5�      #��[  5�      5�      5�      