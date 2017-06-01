Public Class EventCommand
  Implements ICommand

  Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

  Public Sub Execute(parameter As Object) Implements ICommand.Execute
    RaiseEvent Executed(parameter)
  End Sub

  Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
    Return True
  End Function

  Public Event Executed(parameter As Object)

End Class
