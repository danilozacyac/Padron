   M S B A M L   `   `   `  ����E  AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls  5      
 Padron   Padron.PadronWin   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      SCustom@http://schemas.microsoft.com/winfx/2006/xaml/presentation/ribbon     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a
 ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
   5      -   5        $
��1136��5      $	��500��5	      $	��500��5
      $��Padrón de distribución��5      )  #Telerik.Windows.Controls.WindowBase    WindowState$  	Maximized=�5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��25 �6   .+�5   
   ,  &Telerik.Windows.Controls.RadRibbonView  -   5          Name$ BarraPrincipal��5      $��0,0,0,0q�5      $��Stretch=�5      $��Stretch=�5         ApplicationName$G APadrón de Distribución del Semanario Judicial de la Federación��5         HelpButtonVisibility$ Visible=�5         IsMinimizable .  .@�5      +  %Telerik.Windows.Controls.RadRibbonTab  -   5          Name$ 	Principal��5   #   $��	Principal��66   .@�5      -  'Telerik.Windows.Controls.RadRibbonGroup  $��
Catálogos��6)   .@�5      .  (Telerik.Windows.Controls.RadRibbonButton  -   5          Name$ BtnCatObras��5       $��Obras��5"         
LargeImage$ Resources/catObras.png��5#      	   Size$	 Large=�5$      

 � Tag$
 1��5%        5&      -   5        $ BtnCatOrganismos��5'      $��
Organismos��5)      $ Resources/building.png��5*      $	 Large=�5+      $
 4��5,        5-      -   5        $ BtnCatTitulares��5.      $��	Titulares��50      $  Resources/catTitulares.png��51      $	 Large=�52      $
 3��53        54      -   5        $ 	BtnKiosko��55      $��Kiosko��57      $# Resources/ibooks-icon-200.png��58      $	 Large=�59      $
 2��5:      5<        5=      $��Generar��6)   .@�5>        -   5        $ BtnPlantillas��5?      $��Padrón��5A      $ Resources/ImaPadron.png��5B      $	 Large=�5C      $
 5��5D      5E        5F      $��
Histórico��6)   .@�5G        -	   5        $ BtnVerPadrones��5H      $ Resources/ImaPadron.png��5J      $	 Large=�5K      $
 8��5L      .��5M      ��  �� Orientation$ Vertical=�6%   .+�5N      �� $��Center=�6(   $	��Ver��6E   �� 5O      $��Center=�6(   $��padrones��6E   5P      5Q      5R        5S      .@�5T        -
   5        $ BtnManteniento��5U      $��Mantenimiento��5W      {�. $! Resources/mantenimiento.png��5Y      $	 Large=�5Z        5[      -   5        $ BtnCreaPlantilla��5\      {�. $ Resources/duplicate.png��5_      $	 Large=�5`      $	
 802��5a      .��5b      �� $ Vertical=�6%   .+�5c      �� $��Center=�6(   $��Generar��6E   �� 5d      $��Center=�6(   $��	plantilla��6E   5e      5f        5i      -   5        $ BtnPermisos��5j      $��Permisos��5l      {�. $# Resources/1425495851_lock.png��5n      $	 Large=�5o      $	
 999��5p      5q        5r      5t        5u      -   5        $ TabObras��5u   #   $��Obras��65   .@�5v        -   5          Name$ ObraAcciones��5v   )   $��Acciones��6?   .@�5w        -   5        $ VerObra��5x      $��Ver Información��5z      $ Resources/Books_128.png��5{      $	 Large=�5|      $	
 101��5}        5~      -   5        $ AgregarObra��5      $��Agregar��5�      $! Resources/Books_128_add.png��5�      $	 Large=�5�      $	
 102��5�        5�      -   5        $ 
EditarObra��5�      $��	Modificar��5�      $" Resources/Books_128_edit.png��5�      $	 Large=�5�      $	
 103��5�        5�      -   5        $ EliminarObra��5�      $��
Desactivar��5�      $$ Resources/Books_128_delete.png��5�      $	 Large=�5�      $	
 104��5�        5�      -   5        $ ActivarObra��5�      $��Activar��5�      $! Resources/Books_128_add.png��5�      $	 Large=�5�      $	
 105��5�      $y�	Collapsed=�5�        5�      -   5        $ ObrasDesctivadas��5�      $��Obras Inactivas��5�      $! Resources/Books_128_add.png��5�      $	 Large=�5�      $	
 106��5�        5�      -   5        $ ObrasActivas��5�      $��Obras Activas��5�      $! Resources/Books_128_add.png��5�      $	 Large=�5�      $	
 107��5�      $y�	Collapsed=�5�      5�        5�      -   5        $ ObraExportar��5�   )   $��Exportar��6?   .@�5�      6  0Telerik.Windows.Controls.RadRibbonDropDownButton  -   5          Name$ BtnExportObras��5�         
LargeImage$ Resources/word_128.png��5�         Size$ Large=�5�      $	
 120��5�         Text$ Lista de Obras��5�      0  *Telerik.Windows.Controls.RadDropDownButton   DropDownContent 5�      -  'Telerik.Windows.Controls.RadContextMenu  5�      $��0q�65   '	  !Telerik.Windows.Controls.MenuBase 	  IconColumnWidth$ 24Q�6I   .@�5�   "   *
  $Telerik.Windows.Controls.RadMenuItem
  -   5         
 Name$ BtnListaObraToWord��5�   %   $��Lista en Word��5�   %    
  Icon 5�   &   �� 5�   *   $��Resources/word_32.png��60   $
��Fill=�6O   5�   #   
  5�   "   -   5        $ BtnListaObraToExcel��5�   %   $��Lista en Excel��5�   %    5�   &   �� 5�   *   $��Resources/excel_32.png��60   $
��Fill=�6P   5�   #   
  5�   "   -   5        $ BtnListaObraToPdf��5�   %   $��Lista en PDF��5�   %    5�   &   �� 5�   *   $��Resources/pdf_32.png��60   $
��Fill=�6N   5�   #   5�      5�      5�      5�        5�      -   5        $ TabTitulares��5�   #   $��	Titulares��69   .@�5�        $��Titular��6)   .@�5�        -   5        $ InfoTitular��5�      $ Resources/Author.png��5�      $	 Large=�5�      $	
 301��5�      .��5�      �� $ Vertical=�6%   .+�5�      �� $��Center=�6(   $	��Ver��6E   �� 5�      $��Center=�6(   $��información��6E   5�      5�        5�      -   5        $ AgregaTitular��5�      $��Agregar��5�      $ Resources/Author_add.png��5�      $	 Large=�5�      $	
 302��5�        5�      -   5        $ ModificaTitular��5�      $��	Modificar��5�      $ Resources/Author_edit.png��5�      $	 Large=�5�      $	
 303��5�        5�      -   5        $ BtnSinDistribucion��5�      $! Resources/Author_delete.png��5�      $	 Large=�5�      $	
 304��5�      .��5�      �� $ Vertical=�6%   .+�5�      �� $��Center=�6(   $��	No quiere��6E   �� 5�      $��Center=�6(   $��distribución��6E   5�      5�        5�      -    5        $ BtnTitularesActivos��5�      $��Activos��5�      $ Resources/lawyers.png��5�      $	 Large=�5�      $	
 305��5�      $y�	Collapsed=�5�        5      -!   5        $ BtnTitularesJubilados��5     $ Resources/old_256.png��5     $	 Large=�5     $	
 306��5     .��5     �� $ Vertical=�6%   .+�5     �� $��Center=�6(   $��Jubilados o��6E   �� 5     $��Center=�6(   $��
fallecidos��6E   5	     5
       5     -"   5        $ BtnTitularEsAutor��5     $  Resources/Author_books.png��5     $	 Large=�5     $	
 307��5     .��5     �� $ Vertical=�6%   .+�5     �� $��Center=�6(   $��Marcar como ��6E   �� 5     $��Center=�6(   $��autor��6E   5     5     5       5     $��Historiales��6)   $y�Visible=�6>   .@�5       -#   5        $ HistorialTitular��5     $��Trayectoria��5     $  Resources/schedule_128.png��5     $	 Large=�5     $	
 310��5       5      -$   5        $ HistorialFuncObras��5!     $  Resources/schedule_128.png��5#     $	 Large=�5$     $	
 311��5%     .��5&     �� $ Vertical=�6%   .+�5'     �� $��Center=�6(   $��Obras��6E   �� 5(     $��Center=�6(   $��	Recibidas��6E   5)     5*     5+       5-     $��Reportes��6)   .@�5.       -%   5        $ BtnIncluidosTodo��5/     $ Resources/mailp_128.png��51     $	 Large=�52     $	
 320��53     .��54     �� $ Vertical=�6%   .+�55     �� $��Center=�6(   $��Incluidos en��6E   �� 56     $��Center=�6(   $��todos los tirajes��6E   57     58     59     5;       5<     -&   5        $ TabOrganismos��5<  #   $��
Organismos��6:   .@�5=       $��Matenimiento��6)   .@�5>       -'   5        $ InfoOrganismo��5?     $ Resources/Building.png��5A     $	 Large=�5B     $	
 401��5C     .��5D     �� $ Vertical=�6%   .+�5E     �� $��Center=�6(   $	��Ver��6E   �� 5F     $��Center=�6(   $��información��6E   5G     5H       5I     -(   5        $ AgregarOrganismo��5J     $��Agregar��5L     $  Resources/Building_add.png��5M     $	 Large=�5N     $	
 402��5O       5P     -)   5        $ ModificaOrganismo��5Q     $��	Modificar��5S     $! Resources/Building_edit.png��5T     $	 Large=�5U     $	
 403��5V       5W     -*   5        $ BtnActivarOrganismo��5X     $��Activar��5Z     $ Resources/Building.png��5[     $	 Large=�5\     $	
 404��5]     $y�	Collapsed=�5^       5_     -+   5        $ DesactivaOrganismo��5`     $��
Desactivar��5b     $# Resources/Building_delete.png��5c     $	 Large=�5d     $	
 405��5e       5f     -,   5        $ BtnOrgActivados��5g     $��Activos��5i     $ Resources/Building.png��5j     $	 Large=�5k     $y�	Collapsed=�5l       5m     --   5        $ BtnOrgDesactivados��5n     $��Desactivados��5p     $ Resources/Building.png��5q     $	 Large=�5r     5t       5u     $��Herramientas��6)   .@�5v       -.   5        $ BtnLimpiaFiltro��5w     $ Resources/escoba.png��5y     $	 Large=�5z     $	
 410��5{     .��5|     �� $ Vertical=�6%   .+�5}     �� $��Center=�6(   $��Limpiar��6E   �� 5~     $��Center=�6(   $��filtros��6E   5     5�     5�       5�     $��Historiales��6)   $y�	Collapsed=�6>   .@�5�       -/   5        $ HistorialOrganismo��5�     $  Resources/schedule_128.png��5�     $	 Large=�5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��	Historial��6E   �� 5�     $��Center=�6(   $��	organismo��6E   5�     5�       5�     -0   5        $ BtnHistOrgObras��5�     $  Resources/schedule_128.png��5�     $	 Large=�5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Obras��6E   �� 5�     $��Center=�6(   $��	recibidas��6E   5�     5�     5�       5�     $��Reportes��6)   .@�5�       -1   5        $ BtnImprDetalleOrg��5�     $ Resources/detail.png��5�     $ Large=�5�     $	
 440��5�     $  Imprimir detalle organismo��5�      5�       5�     $��0q�65   $ 24Q�6I   .@�5�  "   
  -2   5        $ BtnDetImprSeleccion��5�  %   $��
Selección��5�  %   $	
 441��5�  %   
  5�  "   -3   5        $ BtnDetImprVista��5�  %   $��Listado��5�  %   $	
 442��5�  %   
  5�  "   -4   5        $ BtnDetImprTodo��5�  %   $��Lista en PDF��5�  %   $	
 443��5�  %   5�     5�       5�     -5   5        $! BtnGeneraEtiquetasSelection��5�     $ Resources/label.png��5�     $ Large=�5�     $	
 444��5�     $ Imprimir Etiquetas��5�      5�       5�     $��0q�65   $ 24Q�6I   .@�5�  "   
  -6   5        $ BtnEtiquetaSeleccion��5�  %   $��
Selección��5�  %   $	
 446��5�  %   
  5�  "   -7   5        $ BtnEtiquetaVista��5�  %   $��Listado��5�  %   $	
 447��5�  %   5�     5�       5�     -8   5        $ BtnTotalTitulares��5�     $ Resources/Boy_Girl.png��5�     $	 Large=�5�     $	
 449��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Total titulares��6E   �� 5�     $��Center=�6(   $��por tipo organismo��6E   5�     5�       5�     -9   5        $ BtnTotalSecretarios��5�     $ Resources/boygirl_128.png��5�     $	 Large=�5�     $	
 445��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Total��6E   �� 5�     $��Center=�6(   $��Secretarios��6E   5�     5�       5�     -:   5        $ BtnTotalOrganismos��5�     $ Resources/mexico_128.png��5�     $	 Large=�5�     $	
 448��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Total��6E   �� 5�     $��Center=�6(   $��
Organismos��6E   5�     5�     5�     5�       5�     -;   5        $ 	TabPadron��5�  #   $��Padrón��66   .@�5�       -<   5        $ 
GpoPrepara��5�  )   $��Preparación��6=   .@�5�       -=   5        $ IncluyeTitular��5�     $��Incluir titular��5�     $ Resources/titular.png��5�     $	 Large=�5�     $	
 501��5�       5�     ->   5        $ ExcluyeTitular��5�     $��Excluir titular��5�     $ Resources/titular.png��5�     $	 Large=�5�     $	
 502��5�       5�     -?   5        $ BtnObraEspecial��5�     $ Resources/straTrophy.png��5�     $	 Large=�5�     $	
 504��5       � ToolTip$v pEn las obras especiales se permite modificar el número de ejemplares que se asignan para los almacenes y ventas��5     .��5     �� $ Vertical=�6%   .+�5     �� $��Center=�6(   $
��Obra��6E   �� 5     $��Center=�6(   $��Especial��6E   5     5       5     -@   5        $ GuardarPadron��5     $ Resources/save.png��5
     $	 Large=�5     $	
 503��5     .��5     �� $ Vertical=�6%   .+�5     �� $��Center=�6(   $��Generar��6E   �� 5     $��Center=�6(   $��Padrón��6E   5     5     5       5     -A   5        $ GpoPapeleria��5  )   $��
Papelería��6?   .@�5       -B   5        $ BtnSinNumAcuerdo��5     $! Resources/AcuerdoCancel.png��5     $	 Large=�5     $	
 519��5     .��5     �� $ Vertical=�6%   .+�5     �� $��Center=�6(   $��Sin número��6E   �� 5     $��Center=�6(   $��
de acuerdo��6E   5     5       5     -C   5        $ BtnListadosEntrega��5      $ Resources/lines_128.png��5!     $ Large=�5"     $	
 510��5#     $ Listados de entrega��5$      5%       5&     $��0q�65   $ 24Q�6I   .@�5'  "   
  -D   5        $ BtnFirmaCorte��5(  %   $
��SCJN��5*  %   $	
 511��5+  %   
  5,  "   -E   5        $ BtnFirmaSanLazaro��5-  %   $��San Lázaro��5/  %   $	
 512��50  %   
  51  "   -F   5        $ BtnFirmaMetropolitana��52  %   $��Área Metropolitana��54  %   $	
 513��55  %   
  56  "   -G   5        $ BtnForaneos��57  %   $��Foraneos��59  %   $	
 514��5:  %   5;     5=       5?     -H   5        $ BtnGeneraAcuses��5@     $ Resources/acuse_128.png��5B     $	 Large=�5C     $	
 515��5D     .��5E     �� $ Vertical=�6%   .+�5F     �� $��Center=�6(   $��Generar��6E   �� 5G     $��Center=�6(   $��Oficios��6E   5H     5I       5J     -I   5        $ BtnGeneraEtiquetas��5K     $ Resources/label.png��5M     $	 Large=�5N     $	
 516��5O     .��5P     �� $ Vertical=�6%   .+�5Q     �� $��Center=�6(   $��Generar��6E   �� 5R     $��Center=�6(   $��	Etiquetas��6E   5S     5T       5U     -J   5        $ BtnListadoContraloria��5V     $# Resources/contraloria_128.png��5X     $	 Large=�5Y     $	
 517��5Z     .��5[     �� $ Vertical=�6%   .+�5\     �� $��Center=�6(   $��Listado��6E   �� 5]     $��Center=�6(   $��Contraloría��6E   5^     5_       5`     -K   5        $ BtnVerDistribucion��5a     $# Resources/searchtable_128.png��5c     $	 Large=�5d     $	
 518��5e     .��5f     �� $ Vertical=�6%   .+�5g     �� $��Center=�6(   $	��Ver��6E   �� 5h     $��Center=�6(   $��Distribución��6E   5i     5j     5k       5l     -L   5        $ GpoRespuesta��5l  )   $��	Respuesta��6?   .@�5m       -M   5        $ RBtnCadenaControl��5n     $! Resources/cadenaControl.png��5p     $	 Large=�5q     $	
 523��5r     .��5s     �� $ Vertical=�6%   .+�5t     �� $��Center=�6(   $��Cadena��6E   �� 5u     $��Center=�6(   $��
de control��6E   5v     5w       5x     -N   5        $ DetalleAcuses��5y     $ Resources/Building.png��5{     $	 Large=�5|     $	
 520��5}     .��5~     �� $ Vertical=�6%   .+�5     �� $��Center=�6(   $��Acuses��6E   �� 5�     $��Center=�6(   $��	de recibo��6E   5�     5�       5�     -O   5        $ BtnPadronPorObra��5�     $ Resources/PorObra.png��5�     $	 Large=�5�     $	
 522��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��
Ver acuses��6E   �� 5�     $��Center=�6(   $��por obra��6E   5�     5�       5�     -P   5        $ BtnClonarPlantilla��5�     $ Resources/repeat_128.png��5�     $	 Large=�5�     $	
 521��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Clonar��6E   �� 5�     $��Center=�6(   $��Padrón��6E   5�     5�     5�       5�     $��Acciones��6)   .@�5�       -Q   5        $ BtnDeletePadron��5�     $ Resources/deleteAll.png��5�     $	 Large=�5�     $	
 524��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Eliminar��6E   �� 5�     $��Center=�6(   $��Padrón��6E   5�     5�     5�       5�     $��Reportes��6)   .@�5�       -R   5        $ BtnDistNacional��5�     $ Resources/mapaDist.png��5�     $	 Large=�5�     $	
 525��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Distribución��6E   �� 5�     $��Center=�6(   $��
por Estado��6E   5�     5�     5�     5�       5�     -S   5        $ 	TabKiosko��5�  #   $��Kiosko��66   .@�5�       $��
Catálogos��6)   .@�5�       -T   5        $ BtnKioskoObras��5�     $��Obras��5�     $! Resources/bookshelf_128.png��5�     $	 Large=�5�     $	
 201��5�       5�     -U   5        $ BtnKioskoAutores��5�     $��Autores��5�     $ Resources/user1_128.png��5�     $	 Large=�5�     $	
 202��5�       5�     -V   5        $ BtnEstructura��5�     $ Resources/tree_128.png��5�     $	 Large=�5�     $	
 212��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��
Estructura��6E   �� 5�     $��Center=�6(   $��
del kiosko��6E   5�     5�     5�       5�     -W   5        $ GpObrasKiosko��5�     $��Obras��5�     $y�	Collapsed=�5�     .@�5�       -X   5        $ BtnVerInfoObraKiosko��5�     $  Resources/ObraInfo_128.png��5�     $	 Large=�5�     $	
 210��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $	��Ver��6E   �� 5�     $��Center=�6(   $��información��6E   5�     5�       5�     -Y   5        $ BtnEditObraKiosko��5�     $$ Resources/ObraInfoEdit_128.png��5�     $	 Large=�5�     $	
 211��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Editar��6E   �� 5�     $��Center=�6(   $��información��6E   5�     5�       5�     -Z   5        $ BtnAddImagen��5�     $ Resources/picture.png��5�     $	 Large=�5�     $	
 214��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Agregar��6E   �� 5�     $��Center=�6(   $��imagen��6E   5�     5�     5       5     -[   5        $ GpAutorKiosko��5     $��Autores��5     $y�	Collapsed=�5     .@�5       -\   5        $ BtnVerAutor��5     $��	Ver Autor��5
     $ Resources/author_128.png��5     $	 Large=�5     $	
 230��5       5     -]   5        $ BtnAddAutor��5     $��Agregar Autor��5     $ Resources/Author_add.png��5     $	 Large=�5     $	
 231��5     5       5     -^   5        $ 
GpExportar��5     $��Exportar��5     $y�	Collapsed=�5     .@�5       -_   5        $ BtnPrintSinSintesis��5     $��Sin síntesis��5     $ Resources/SinSintesis.png��5     $	 Large=�5     $	
 240��5      $Z TImprime solamente el título de las obras seleccionadas o asociadas a la selección ��5!       5"     -`   5        $ BtnPrintConSintesis��5#     $��Con síntesis��5%     $ Resources/ConSintesis.png��5&     $	 Large=�5'     $	
 241��5(     $_ YImprime el título y la síntesis de las obras seleccionadas o asociadas a la selección ��5)       5*     -a   5        $ BtnPrintMulSelectK��5+     $" Resources/SelectMultiple.png��5-     $	 Large=�5.     $	
 242��5/     $; 5Permite seleccionar más de un elemento para exportar��50     .��51     �� $ Vertical=�6%   .+�52     �� $��Center=�6(   $��
Selección��6E   �� 53     $��Center=�6(   $��multiple��6E   54     55       56     -b   5        $ BtnPrintOnlySelectK��57     $" Resources/SelectSencilla.png��59     $	 Large=�5:     $	
 243��5;     $F @Permite la selección de un único elemento para su exportación��5<     $y�	Collapsed=�5=     .��5>     �� $ Vertical=�6%   .+�5?     �� $��Center=�6(   $��
Selección��6E   �� 5@     $��Center=�6(   $��simple��6E   5A     5B       5C     -c   5        $ BtnPrintADisposicion��5D     $ Resources/auditor_128.png��5F     $	 Large=�5G     $	
 244��5H     $y�	Collapsed=�5I     .��5J     �� $ Vertical=�6%   .+�5K     �� $��Center=�6(   $��Obras a��6E   �� 5L     $��Center=�6(   $��disposición��6E   5M     5N     5O       5P     -d   5        $ GpEstructura��5Q     $��Estructura Kiosko��5R     $y�	Collapsed=�5S     .@�5T       -e   5        $ BtnEditObraEstructura��5U     $$ Resources/ObraInfoEdit_128.png��5W     $	 Large=�5X     $	
 247��5Y     .��5Z     �� $ Vertical=�6%   .+�5[     �� $��Center=�6(   $��Editar��6E   �� 5\     $��Center=�6(   $��información��6E   5]     5^       5_     -f   5        $ BtnDelClasifObra��5`     $$ Resources/tree2_128_cancel.png��5b     $	 Large=�5c     $	
 246��5d     .��5e     �� $ Vertical=�6%   .+�5f     �� $��Center=�6(   $��Eliminar��6E   �� 5g     $��Center=�6(   $��clasificación��6E   5h     5i       5j     -g   5        $ BtnReasOrden��5k     $# Resources/number_128_blue.png��5m     $	 Large=�5n     $	
 246��5o     .��5p     �� $ Vertical=�6%   .+�5q     �� $��Center=�6(   $��	Reasignar��6E   �� 5r     $��Center=�6(   $��orden��6E   5s     5t       5v     -h   5        $ BtnExportaKiosko��5w     $��Exportar��5y     $" Resources/exportarKiosko.png��5z     $	 Large=�5{     $	
 245��5|     5}       5~     $��Ayuda��6)   .@�5       -i   5        $ BtnCodigoColores��5�     $ Resources/pantone_128.png��5�     $	 Large=�5�     $	
 208��5�     .��5�     �� $ Vertical=�6%   .+�5�     �� $��Center=�6(   $��Código��6E   �� 5�     $��Center=�6(   $��
de colores��6E   5�     5�     5�     5�       5�     -j   5        $ TabReportes��5�  #   $��Reportes��68   .@�5�       $��Estadísticas��6)   5�       5�     -k   5        $ TabMantenimiento��5�     $��4,0,-1,0q�5�     $��Mantenimiento��5�     .@�5�       $��Localidades��6)   .@�5�       -l   5        $ BtnLocalidadesMant��5�     $��Localidades��5�     $ Resources/world_128.png��5�     $	 Large=�5�     $	
 820��5�     �� 5�     -m   5         ��Name$ StackPaises��5�  !   $ Vertical=�66   .+�5�       -n   5        $ BtnAgregaPais��5�     $��  Pais��5�     $	 Medium=�5�        
SmallImage$ Resources/add_24.png��5�     $	
 821��5�       5�     -o   5        $ BtnEditaPais��5�     $��  País��5�     $	 Medium=�5�     $ Resources/pencil_24.png��5�     $	
 822��5�       5�     -p   5        $ BtnEliminaPais��5�     $��  País��5�     $	 Medium=�5�     $ Resources/error_24.png��5�     $	
 823��5�     5�     �� 5�     -q   5        $ StackEstados��5�  !   $ Vertical=�67   .+�5�       -r   5        $ BtnAgregaEstado��5�     $��  Estado��5�     $	 Medium=�5�     $ Resources/add_24.png��5�     $	
 824��5�       5�     -s   5        $ BtnEditaEstado��5�     $��  Estado��5�     $	 Medium=�5�     $ Resources/pencil_24.png��5�     $	
 825��5�       5�     -t   5        $ BtnEliminaEstado��5�     $��  Estado��5�     $	 Medium=�5�     $ Resources/error_24.png��5�     $	
 826��5�     5�     �� 5�     -u   5        $ StackCiudades��5�  !   $ Vertical=�68   .+�5�       -v   5        $ BtnAgregaCiudad��5�     $��  Ciudad��5�     $	 Medium=�5�     $ Resources/add_24.png��5�     $	
 827��5�       5�     -w   5        $ BtnEditaCiudad��5�     $��  Ciudad��5�     $	 Medium=�5�     $ Resources/pencil_24.png��5�     $	
 828��5�       5�     -x   5        $ BtnEliminaCiudad��5�     $��  Ciudad��5�     $	 Medium=�5�     $ Resources/error_24.png��5�     $	
 829��5�     5�     5�       5�     $��	Funciones��6)   .@�5�       -y   5        $ BtnFunciones��5�     $��Listado��5�     {�. $ Resources/function.png��5�     $	 Large=�5�     $	
 830��5�     �� 5�     $ Vertical=�6!   .+�5�       -z   5        $ BtnAddFunciones��5�     $��Agregar��5�     {�. $ Resources/add_24.png��5�     $	 Medium=�5�     $	
 831��5�     5�     5�       5�     $��Grado académico��6)   .@�5�       -{   5        $ BtnCatTitulos��5�     $��Listado��5�     {�. $ Resources/degree_blue.png��5�     $	 Large=�5�     $	
 840��5�     �� 5�     $ Vertical=�6!   .+�5        -|   5        $ BtnAddTitulo��5     $��Agregar��5     {�. $ Resources/add_24.png��5     $	 Medium=�5     $	
 841��5     5     5	       5
     $��Oficios Config��6)   .@�5       -}   5        $ BtnConfOficio��5     $��Conf. Oficios��5     {�. $ Resources/configPaper.png��5     $	 Large=�5     $	
 842��5     5     5     5     ]� 5  
   -~   5         ]�Name$ CentralPanel��5     $��1��5     $��Stretch=�5     $��Stretch=�5     ��. 5     �� 5  
   -   5         ��Name$ 
LblVersion��5      $��2��5!     $
��Auto��5"     $��23��5#     $��0q�5$     $��	Versión:��5%     5'     5(     