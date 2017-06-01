Public Class EmojiUWP
  Inherits Emoji
  Implements INotifyPropertyChanged

  Public ReadOnly Property Model As EmojiSearchModelUWP

  Public Sub New(name As String,
                 keywords As IEnumerable(Of String),
                 value As String,
                 fitzpatrickScale As Boolean,
                 category As String,
                 model As EmojiSearchModelUWP)
    MyBase.New(name, keywords, value, fitzpatrickScale, category)
    If model Is Nothing Then
      Throw New ArgumentNullException(NameOf(model))
    End If
    Me.Model = model
    AddHandler model.PropertyChanged, AddressOf model_PropertyChanged
    _ModifiedValue = GetModifiedValue()
  End Sub

  Private Sub model_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
    If FitzpatrickScale AndAlso
      e.PropertyName = NameOf(EmojiSearchModelUWP.SelectedFitzpatrickEmojiModifier) Then

      _ModifiedValue = GetModifiedValue()
      OnPropertyChanged(NameOf(ModifiedValue))
    End If
  End Sub

  Protected Overridable Function GetModifiedValue() As String
    If FitzpatrickScale Then
      Return Value & Model.SelectedFitzpatrickEmojiModifier.Value
    End If
    Return Value
  End Function

  Private _ModifiedValue As String
  Public ReadOnly Property ModifiedValue As String
    Get
      Return _ModifiedValue
    End Get
  End Property

  Public Event PropertyChanged As PropertyChangedEventHandler _
    Implements INotifyPropertyChanged.PropertyChanged

  Protected Overridable Sub OnPropertyChanged(prop As String)
    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
  End Sub

End Class
