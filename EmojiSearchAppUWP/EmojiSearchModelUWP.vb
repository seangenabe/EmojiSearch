
Imports System.Threading

Public Class EmojiSearchModelUWP
  Inherits EmojiSearchModel

  Public Property EmojisView As IEnumerable(Of Emoji)
  Private Const DEBOUNCE_DURATION = 400

  Public Sub New()
    MyBase.New()
    EmojisView = Emojis.AsEnumerable()
    _QueryDebounced = Query
  End Sub

  Protected Overrides Sub OnPropertyChanged(e As PropertyChangedEventArgs)
    If e.PropertyName = NameOf(Query) Then
      Dim q = Query.Trim()
      If String.IsNullOrEmpty(q) Then
        EmojisView = Emojis.AsEnumerable()
      Else
        EmojisView = From emoji In Emojis
                     From keyword In emoji.Keywords
                     Where emoji.Name.StartsWith(q) OrElse
                       keyword.StartsWith(q)
                     Select emoji
      End If
      OnPropertyChanged(New PropertyChangedEventArgs(NameOf(EmojisView)))
    End If
    MyBase.OnPropertyChanged(e)
  End Sub

  Private _QueryDebounced As String
  Private _LastUpdate As Integer = Integer.MinValue
  Public Property QueryDebounced As String
    Get
      Return _QueryDebounced
    End Get
    Set(value As String)
      If _QueryDebounced <> value Then
        _QueryDebounced = value
        If _LastUpdate = Integer.MaxValue Then
          _LastUpdate = Integer.MinValue
        End If
        Dim current = Interlocked.Increment(_LastUpdate)
        Dim ac = Async Sub()
                   Await Task.Delay(DEBOUNCE_DURATION)
                   If current = _LastUpdate Then
                     SyncLock Me
                       Query = value
                     End SyncLock
                   End If
                 End Sub
        ac()
      End If
    End Set
  End Property

End Class
