Imports LiteDB
Imports System.IO
Imports Newtonsoft.Json.Linq

Module Module1

  Sub Main()
    File.Create("Emoji.db").Dispose()
    Using db As New LiteDatabase("Emoji.db")
      Dim emojisCollection = db.GetCollection("emojis")

      ' Create index
      emojisCollection.EnsureIndex("keywords")

      Dim emojisText = File.ReadAllText("emojis.json")
      Dim emojis As JObject = JObject.Parse(emojisText)
      For Each e As JToken In emojis.PropertyValues
        Dim emoji2 = Convert(e)
        emojisCollection.Insert(emoji2)
      Next
    End Using
  End Sub

  Function Convert(token As JToken) As BsonDocument
    Dim obj = DirectCast(token, JObject)

    ' Extract JSON object contents
    Dim keywords As String() =
      (From keyword In DirectCast(token!keywords, JArray) Select CStr(keyword)) _
      .ToArray()
    Dim ch = CStr(token!char)
    Dim fitzpatrick_scale = CBool(token!fitzpatrick_scale)
    Dim category = CStr(token!category)

    ' Build BsonDocument
    Dim document As New BsonDocument()
    Dim keywordsArray As New BsonArray(From keyword In keywords Select New BsonValue(keyword))
    document.Set("keywords", keywordsArray)
    document.Set("char", New BsonValue(ch))
    document.Set("fitzpatrick_scale", New BsonValue(fitzpatrick_scale))
    document.Set("category", New BsonValue(category))

    Return document
  End Function

End Module
