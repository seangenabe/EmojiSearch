﻿Imports LiteDB
Imports Windows.ApplicationModel.DataTransfer

Public NotInheritable Class MainPage
  Inherits Page

  Public Sub New()
    InitializeComponent()
    EmojiItemsControl.XYFocusKeyboardNavigation = XYFocusKeyboardNavigationMode.Enabled
    ItemSelectedCommand = New ItemSelectedCommand()
    AddHandler ItemSelectedCommand.Executed, AddressOf OnItemSelected
  End Sub

  Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
    QueryTextBox.Focus(FocusState.Programmatic)
    MyBase.OnNavigatedTo(e)
  End Sub

  Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
    Dim app As New EmojiSearchModelUWP()
    Using stream = Await Package.Current.InstalledLocation.OpenStreamForReadAsync("Emoji.db")
      Using db As New LiteDatabase(stream)
        app.Load(db)
      End Using
    End Using

    DataContext = app
  End Sub

  Public Property ItemSelectedCommand As ItemSelectedCommand

  Private Sub OnItemSelected(parameter As Object)
    If TypeOf parameter Is Emoji Then
      Dim emoji = DirectCast(parameter, Emoji)
      Dim pkg As New DataPackage() With {.RequestedOperation = DataPackageOperation.Copy}
      pkg.SetText(emoji.Value)
      Clipboard.SetContent(pkg)
    End If
  End Sub

End Class
