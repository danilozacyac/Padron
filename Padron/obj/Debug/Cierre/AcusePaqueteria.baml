   M S B A M L   `   `   `  ����
  Padron#    Padron.Cierre.AcusePaqueteria   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a	 ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation     	 
  5      -   5        $	��300��5      $	��440��5      $��Acuse de paquetería��5      )  #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5
      (  "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5      .��5      � .+�5   
   �� $��	31,80,0,0q�6   $
��Left=�5      $	��Top=�5      $��Número de guía:��5      �� 5   
   -   5         ��Name$ TxtGuia��5      $	��233��5      $��23��5      $��
31,111,0,0q�5      $
��Left=�5      $	��Top=�5      ��5      �� *	NumGuia+ �� Mode$ TwoWay=��� 5   
   $��
31,148,0,0q�6   $
��Left=�5      $	��Top=�5      $��Fecha de entrega:��5      , 
 &Telerik.Windows.Controls.RadDatePicker  5   
   -   5          Name$ DpPaqueteria��5       $	��233��5        $��
31,179,0,0q�5!       $
��Left=�5"       $	��Top=�5#       0 
 *Telerik.Windows.Controls.RadDateTimePicker   SelectedValue 5$       �� *FechaRecPaqueteria+$ TwoWay=��� 5&   
   $��
31,281,0,0q�6   $
��Left=�5'      $	��Top=�5(      $��Archivo de soporte:��5)      �� 5*   
   -   5        $ TxtArchivoPaq��5*      $	��203��5+      $��23��5,      $��
31,312,0,0q�5-      $
��Left=�5.      $	��Top=�5/      {�.  ��51      �� *ArchivoGuia+$ TwoWay=��� 53   
   -   5         ��Name$ BtnArchivoGuia��53      $��25��54      $��239,312,0,0q�55      $
��Left=�56      $	��Top=�57      $	��...��59      �� 5:   
   -   5        $ BtnCancelar��5:      $��90��5;      $��	0,0,15,20q�5<      $��Right=�5=      $��Bottom=�5>      $��Cancelar��5@         
BlueButton#��[  5A      �� 5B   
   -   5        $ 
BtnGuardar��5B      $��88��5C      $��
0,0,110,20q�5D      $��Right=�5E      $��Bottom=�5F      $��Guardar��5H      #��[  5I      �� 5J   
   $��
31,219,0,0q�6   $
��Left=�5K      $	��Top=�5L      $��Recibió��5M      �� 5N   
   $	��233��6   $��23��5O      $��
31,245,0,0q�5P      $
��Left=�5Q      $	��Top=�5R       �� TextWrapping$
 Wrap=�5U      ��5S      �� *QuienRecibe+$ TwoWay=�5U      �� 5V   
   $��	31,10,0,0q�6   $
��Left=�5W      $	��Top=�5X      $��Fecha de envio:��5Y        5Z   
   -   5        $ DpFechaEnvio��5Z       $	��233��5[       $��	31,41,0,0q�5\       $
��Left=�5]       $	��Top=�5^        5_       �� *
FechaEnvio+$ TwoWay=�5b      5c      