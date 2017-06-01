Imports LiteDB
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage

Public NotInheritable Class MainPage
  Inherits Page

  Public Sub New()
    InitializeComponent()
    EmojiItemsControl.XYFocusKeyboardNavigation = XYFocusKeyboardNavigationMode.Enabled

    ItemSelectedCommand = New EventCommand()
    AddHandler ItemSelectedCommand.Executed, AddressOf OnItemSelected

    ResetCommand = New EventCommand()
    AddHandler ResetCommand.Executed, AddressOf Reset
  End Sub

  Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
    QueryTextBox.Focus(FocusState.Programmatic)
    MyBase.OnNavigatedTo(e)
  End Sub

  Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
    Dim model = Me.Model
    Using stream = Await Package.Current.InstalledLocation.OpenStreamForReadAsync("Emoji.db")
      Using db As New LiteDatabase(stream)
        model.Load(db)
      End Using
    End Using

    DataContext = model

    Dim fitzpatrickEmojiModifiersViewSource =
      DirectCast(Resources!FitzpatrickEmojiModifiersViewSource, CollectionViewSource)
    fitzpatrickEmojiModifiersViewSource.View.MoveCurrentTo(model.SelectedFitzpatrickEmojiModifier)
    AddHandler fitzpatrickEmojiModifiersViewSource.View.CurrentChanged,
      AddressOf FitzpatrickEmojiModifiersView_CurrentChanged
  End Sub

  Public Property ItemSelectedCommand As EventCommand
  Public Property ResetCommand As EventCommand
  Public Property FitzpatrickEmojiModifiersViewSource As CollectionViewSource

  Private Sub OnItemSelected(parameter As Object)
    If TypeOf parameter Is EmojiUWP Then
      Dim emoji = DirectCast(parameter, EmojiUWP)
      Dim pkg As New DataPackage() With {.RequestedOperation = DataPackageOperation.Copy}
      pkg.SetText(emoji.ModifiedValue)
      Clipboard.SetContent(pkg)
    End If
  End Sub

  Private Sub Reset()
    QueryTextBox.Text = ""
    QueryTextBox.Focus(FocusState.Programmatic)
  End Sub

  Private Sub FitzpatrickEmojiModifiersView_CurrentChanged(sender As Object, e As Object)
    Dim source = Resources!FitzpatrickEmojiModifiersViewSource
    Dim view = DirectCast(source, CollectionViewSource).View
    Dim currentItem = view.CurrentItem
    Model.SelectedFitzpatrickEmojiModifier = DirectCast(currentItem, FitzpatrickEmojiModifier)
    ApplicationData.Current.RoamingSettings.Values!fitzpatrickIndex = view.CurrentPosition
  End Sub

  Private ReadOnly Property Model As EmojiSearchModelUWP
    Get
      Return DirectCast(Application.Current, App).Model
    End Get
  End Property


End Class
