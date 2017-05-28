Public Class Emoji

  Public ReadOnly Property Keywords As HashSet(Of String)
  Public ReadOnly Property Value As String
  Public ReadOnly Property FitzpatrickScale As Boolean
  Public ReadOnly Property Category As String

  Public Sub New(keywords As IEnumerable(Of String),
                 value As String,
                 fitzpatrickScale As Boolean,
                 category As String)
    Me.Keywords = New HashSet(Of String)(keywords)
    Me.Value = value
    Me.FitzpatrickScale = fitzpatrickScale
    Me.Category = category
  End Sub

End Class
