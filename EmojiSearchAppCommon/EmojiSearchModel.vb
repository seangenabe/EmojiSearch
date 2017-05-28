Imports LiteDB

Public Class EmojiSearchModel
  Implements INotifyPropertyChanged

  Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

  Public Sub New()
    EmojisList = New ObservableCollection(Of Emoji)()
    Emojis = New ReadOnlyObservableCollection(Of Emoji)(EmojisList)
  End Sub

  Public Sub Load(db As LiteDatabase)
    Dim emojisCollection = db.GetCollection("emojis")

    For Each emoji In emojisCollection.FindAll()
      Dim emoji2 As New Emoji((From k As BsonValue In DirectCast(emoji!keywords, BsonArray)
                               Select k.AsString),
                              emoji!char,
                              emoji!fitzpatrick_scale,
                              emoji!category)
      EmojisList.Add(emoji2)
    Next
  End Sub

  Protected Overridable Sub OnPropertyChanged(e As PropertyChangedEventArgs)
    RaiseEvent PropertyChanged(Me, e)
  End Sub

#Region "Properties"

  Private _Query As String

  Public Property Query As String
    Get
      Return _Query
    End Get
    Set(value As String)
      If _Query <> value Then
        _Query = value
        OnPropertyChanged(New PropertyChangedEventArgs(NameOf(Query)))
      End If
    End Set
  End Property

  Private Property EmojisList As ObservableCollection(Of Emoji)
  Public Property Emojis As ReadOnlyObservableCollection(Of Emoji)

#End Region

End Class
