Imports EmojiSearchAppUWP
Imports LiteDB

Public NotInheritable Class MainPage
  Inherits Page

  Public Sub New()
    InitializeComponent()
  End Sub

  Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
    Dim app As New EmojiSearchModel()
    Using stream = Await Package.Current.InstalledLocation.OpenStreamForReadAsync("Emoji.db")
      Using db As New LiteDatabase(stream)
        app.Load(db)
      End Using
    End Using

    DataContext = app
  End Sub

End Class
