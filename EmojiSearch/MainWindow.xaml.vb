
Class MainWindow

  Sub New()
    InitializeComponent()

    Dim app As New EmojiSearchApp()
    app.Load()

    DataContext = app
  End Sub

End Class
