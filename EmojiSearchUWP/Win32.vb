Imports System.Runtime.InteropServices
Imports System.Text
Imports Windows.UI.Core

Public Module Win32
  Public Const HWND_TOPMOST = -1
  Public Const SWP_NOMOVE = 2
  Public Const SWP_NOSIZE = 1

  Public Declare Function SetWindowPos Lib "user32.dll" (hWnd As IntPtr,
                                                         hWndInsertAfter As IntPtr,
                                                         X As Integer,
                                                         Y As Integer,
                                                         cx As Integer,
                                                         cy As Integer,
                                                         uFlags As Integer) As Boolean

  Public Function GetCurrentWindowHandle() As IntPtr
    Return DirectCast(CoreWindow.GetForCurrentThread(), ICoreWindowInterop).WindowHandle
  End Function

  <ComImport(),
    Guid("45D64A29-A63E-4CB6-B498-5781D298CB4F"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Private Interface ICoreWindowInterop
    ReadOnly Property WindowHandle As IntPtr
    WriteOnly Property MessageHandled As Boolean
  End Interface

  Public Const GWL_EXSTYLE As Integer = -20
  Public Const WS_EX_NOACTIVATE As Long = &H0800_0000L

  Public Declare Function SetWindowLongPtrW Lib "user32.dll" (hWnd As IntPtr,
                                                              nIndex As Integer,
                                                              dwNewLong As Long) As Long
  Public Declare Function GetWindowLongPtrW Lib "user32.dll" (hWnd As IntPtr,
                                                              nIndex As Integer) As Long

  Public Declare Function GetWindowTextW Lib "user32.dll" (
    hWnd As IntPtr,
    <MarshalAs(UnmanagedType.LPTStr)> ByRef lpString As StringBuilder,
    nMaxCount As Integer
    ) As Integer

End Module
