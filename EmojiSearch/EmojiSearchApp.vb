Imports System.Collections.ObjectModel
Imports LiteDB

Public Class EmojiSearchApp
  Inherits DependencyObject

  Public Sub New()
    EmojisList = New ObservableCollection(Of Emoji)()
    Emojis = New ReadOnlyObservableCollection(Of Emoji)(EmojisList)
    EmojisView = New CollectionView(Emojis)
  End Sub

  Protected Overrides Sub OnPropertyChanged(e As DependencyPropertyChangedEventArgs)
    If e.Property Is QueryProperty Then
      Dim value = CStr(e.NewValue)
      EmojisView.Filter =
        Function(emoji)
          Return DirectCast(emoji, Emoji).Keywords.Any(Function(keyword)
                                                         Return keyword.StartsWith(value)
                                                       End Function)
        End Function
    End If
    MyBase.OnPropertyChanged(e)
  End Sub

  Public Sub Load()
    Using db As New LiteDatabase("Emoji.db")
      Dim emojisCollection = db.GetCollection("emojis")

      For Each emoji In emojisCollection.FindAll()
        Dim emoji2 As New Emoji((From k As BsonValue In DirectCast(emoji!keywords, BsonArray)
                                 Select k.AsString),
                                emoji!char,
                                emoji!fitzpatrick_scale,
                                emoji!category)
        EmojisList.Add(emoji2)
      Next
    End Using
  End Sub

#Region "Properties"

  Public Shared ReadOnly QueryProperty As DependencyProperty =
    DependencyProperty.Register(NameOf(Query),
                                GetType(String),
                                GetType(EmojiSearchApp))

  Public Property Query As String
    Get
      Return CStr(GetValue(QueryProperty))
    End Get
    Set(value As String)
      SetValue(QueryProperty, value)
    End Set
  End Property

  Private Property EmojisList As ObservableCollection(Of Emoji)
  Public Property Emojis As ReadOnlyObservableCollection(Of Emoji)
  Public ReadOnly Property EmojisView As CollectionView

#End Region

End Class
