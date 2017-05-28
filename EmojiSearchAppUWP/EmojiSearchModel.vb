
Public Class EmojiSearchModelUWP
  Inherits EmojiSearchModel

  Public ReadOnly Property EmojisView As CollectionViewSource

  Public Sub New()
    MyBase.New()
    EmojisView = New CollectionViewSource() With {.Source = Me.Emojis}
  End Sub

End Class
