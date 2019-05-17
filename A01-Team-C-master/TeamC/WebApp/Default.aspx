<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <img src="TeamLogo/team-logo.png" />
        <p class="lead">Members: Yi, Madison, Matthew</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Site Sections</h2>
            <p>Yi: Sales</p>
            <p>Madison: Purchasing</p>
            <p style="text-decoration: line-through">Gagan: Purchasing</p>
            <p>Matthew: Jobing</p>
        </div>
        <div class="col-md-4">
            <h2>Shared Components</h2>
            <p>1. Creating the solution: Yi</p>
            <p>2. Add navigation: Gagan</p>
            <p>3. Generate entity classes: Yi</p>
            <p>4. Add common user controls: Matthew</p>
            <p>5. Provide documentation: Madison</p>
        </div>
        <div class="col-md-4">
            <h2>Known Bugs</h2>
            <p>List of known bugs in your project, including portions of your lab that are incomplete. List these bugs under a heading that specifies the release version of your application (such as v 1.0.0 - Beta</p>
            <h3>Release Version: v1.0.0</h3>
            <h4>Jobing</h4>
            <p>No known bugs</p>
            <h4>Purchasing</h4>
            <ul>
                <li>Place Functionality: doesn't update QuantityOnOrder in Parts table (Found in PurchasingController)</li>
                <li>Delete Functionality: doesn't delete </li>
            </ul>
            <h4>Sales</h4>
            <p>No known bugs</p>
        </div>
    </div>

</asp:Content>
