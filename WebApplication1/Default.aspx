<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
     <script>
         function callUrl(url) {
             $.ajax({
                 type: 'GET',
                 url: url,
                 error: function (xhr, textStatus, error) {
                     requestError(xhr, textStatus, error, url);
                 },
             });
             
         };
         $(document).ready(function () {
             callUrl("/LogInformation.aspx?simulationGameID=6df69434-c41f-48a7-95d6-f63915112be4");
         });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Each time the Page is loaded the inforation are saved
    </div>
    </form>
</body>
</html>
