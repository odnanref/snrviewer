
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	private global::Gtk.Action FileAction;
	private global::Gtk.Action ExitAction;
	private global::Gtk.Action ConfigurationAction;
	private global::Gtk.Action HelpAction;
	private global::Gtk.Action AboutAction1;
	private global::Gtk.Action RemoteIpAction;
	private global::Gtk.Action LoadFromFileAction;
	private global::Gtk.Action RunQueryAction;
	private global::Gtk.Action NewEquipmentAction;
	private global::Gtk.Action Action;
	private global::Gtk.VBox vbox1;
	private global::Gtk.MenuBar menubar1;
	private global::Gtk.HBox hbox1;
	private global::Gtk.VBox vbox2;
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	private global::Gtk.TreeView equipmentList;
	private global::Gtk.DrawingArea drawingarea4;
	private global::Gtk.VBox vbox3;
    
	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("_File"), null, null);
		this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("File");
		w1.Add (this.FileAction, null);
		this.ExitAction = new global::Gtk.Action ("ExitAction", global::Mono.Unix.Catalog.GetString ("_Exit"), null, null);
		this.ExitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Exit");
		w1.Add (this.ExitAction, null);
		this.ConfigurationAction = new global::Gtk.Action ("ConfigurationAction", global::Mono.Unix.Catalog.GetString ("Configuration"), null, null);
		this.ConfigurationAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Configuration");
		w1.Add (this.ConfigurationAction, null);
		this.HelpAction = new global::Gtk.Action ("HelpAction", global::Mono.Unix.Catalog.GetString ("Help"), null, null);
		this.HelpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
		w1.Add (this.HelpAction, null);
		this.AboutAction1 = new global::Gtk.Action ("AboutAction1", global::Mono.Unix.Catalog.GetString ("About"), null, null);
		this.AboutAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("About");
		w1.Add (this.AboutAction1, null);
		this.RemoteIpAction = new global::Gtk.Action ("RemoteIpAction", global::Mono.Unix.Catalog.GetString ("Remote Ip"), null, null);
		this.RemoteIpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Remote Ip");
		w1.Add (this.RemoteIpAction, null);
		this.LoadFromFileAction = new global::Gtk.Action ("LoadFromFileAction", global::Mono.Unix.Catalog.GetString ("Load from file"), null, null);
		this.LoadFromFileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Load from file");
		w1.Add (this.LoadFromFileAction, null);
		this.RunQueryAction = new global::Gtk.Action ("RunQueryAction", global::Mono.Unix.Catalog.GetString ("Run query"), null, null);
		this.RunQueryAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Run query");
		w1.Add (this.RunQueryAction, null);
		this.NewEquipmentAction = new global::Gtk.Action ("NewEquipmentAction", global::Mono.Unix.Catalog.GetString ("_New Equipment"), null, null);
		this.NewEquipmentAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_New Equipment");
		w1.Add (this.NewEquipmentAction, null);
		this.Action = new global::Gtk.Action ("Action", null, null, null);
		w1.Add (this.Action, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("SnrViewer");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.DefaultHeight = 600;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='RunQueryAction' action='RunQueryAction'/><menuitem name='ExitAction' action='ExitAction'/></menu><menu name='ConfigurationAction' action='ConfigurationAction'><menuitem name='RemoteIpAction' action='RemoteIpAction'/><menuitem name='LoadFromFileAction' action='LoadFromFileAction'/><menuitem name='NewEquipmentAction' action='NewEquipmentAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='AboutAction1' action='AboutAction1'/></menu><menu/></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.vbox1.Add (this.menubar1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar1]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.equipmentList = new global::Gtk.TreeView ();
		this.equipmentList.CanFocus = true;
		this.equipmentList.Name = "equipmentList";
		this.GtkScrolledWindow.Add (this.equipmentList);
		this.vbox2.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow]));
		w4.Position = 0;
		// Container child vbox2.Gtk.Box+BoxChild
		this.drawingarea4 = new global::Gtk.DrawingArea ();
		this.drawingarea4.Name = "drawingarea4";
		this.vbox2.Add (this.drawingarea4);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.drawingarea4]));
		w5.Position = 1;
		this.hbox1.Add (this.vbox2);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
		w6.Position = 0;
		// Container child hbox1.Gtk.Box+BoxChild
		this.vbox3 = new global::Gtk.VBox ();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		this.hbox1.Add (this.vbox3);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox3]));
		w7.Position = 1;
		this.vbox1.Add (this.hbox1);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
		w8.Position = 1;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 553;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.ExitAction.Activated += new global::System.EventHandler (this.btSair);
		this.RunQueryAction.Activated += new global::System.EventHandler (this.btRunQuery);
		this.NewEquipmentAction.Activated += new global::System.EventHandler (this.newEquipmentAction);
	}
}
