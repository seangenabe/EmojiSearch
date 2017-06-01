Public Class FitzpatrickEmojiModifier

  Public ReadOnly Property Value As String
  Public ReadOnly Property DisplayValue As String

  Public Sub New(value As String)
    Me.Value = value
    DisplayValue = value
  End Sub

  Public Sub New(value As String, displayValue As String)
    Me.Value = value
    Me.DisplayValue = displayValue
  End Sub

  Public Shared ReadOnly Property Unmodified As FitzpatrickEmojiModifier =
    New FitzpatrickEmojiModifier("", "")

End Class
