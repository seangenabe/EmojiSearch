Imports LiteDB

Public Class EmojiSearchModel
  Implements INotifyPropertyChanged

  Public Event PropertyChanged _
    As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
  Public ReadOnly Property FitzpatrickEmojiModifiers _
    As ReadOnlyCollection(Of FitzpatrickEmojiModifier)

  Public Sub New()
    EmojisList = New ObservableCollection(Of Emoji)()
    Emojis = New ReadOnlyObservableCollection(Of Emoji)(EmojisList)

    Dim modifiers As New List(Of FitzpatrickEmojiModifier)()
    modifiers.Add(FitzpatrickEmojiModifier.Unmodified)
    modifiers.AddRange(From ch In {"🏻", "🏼", "🏽", "🏾", "🏿"}
                       Select New FitzpatrickEmojiModifier(ch))
    FitzpatrickEmojiModifiers = New ReadOnlyCollection(Of FitzpatrickEmojiModifier)(modifiers)
  End Sub

  Protected Iterator Function FetchFromDatabase(db As LiteDatabase) As IEnumerable(Of Emoji)
    Dim emojisCollection = db.GetCollection("emojis")

    Dim list As New List(Of Emoji)
    For Each emoji In emojisCollection.FindAll()
      Yield CreateNewEmoji(emoji!name,
                           (From k As BsonValue In DirectCast(emoji!keywords, BsonArray)
                            Select k.AsString),
                           emoji!char,
                           emoji!fitzpatrick_scale,
                           emoji!category)
    Next
  End Function

  Protected Overridable Function CreateNewEmoji(name As String,
                                                keywords As IEnumerable(Of String),
                                                ch As String,
                                                fitzpatrickScale As Boolean,
                                                category As String) As Emoji
    Return New Emoji(name, keywords, ch, fitzpatrickScale, category)
  End Function

  Protected Overridable Sub LoadCore(db As LiteDatabase, source As IEnumerable(Of Emoji))
    For Each emoji In source
      EmojisList.Add(emoji)
    Next
  End Sub

  Public Sub Load(db As LiteDatabase)
    LoadCore(db, FetchFromDatabase(db))
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
