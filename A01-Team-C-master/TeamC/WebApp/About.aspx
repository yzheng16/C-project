<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Team Members</h3>
    <ul>
        <li>Matthew</li>
        <li>Madison</li>
        <li>Yi</li>
    </ul>
    <h3>Defaul Security Roles</h3>
    <ul>
        <li>Administrators </li>
        <li>Customers </li>
        <li>Employees </li>
        <li>Purchasing</li>
        <li>Receiving</li>
        <li>RegisteredUsers</li>
        <li>Services</li>
    </ul>
    <h3>Default Users</h3>
    <ul>
        <li>UserName: Webmaster <br />Password: Pa$$w0rd</li>
    </ul>
    <h3>Default Connection strings</h3>
    <ul>
        <li> name="DefaultConnection" connectionString="Data Source=.;Initial Catalog=eBikes;Integrated Security=True" providerName="System.Data.SqlClient"</li>
        <li> name="eBikeDB" connectionString="data source=.;initial catalog=eBikes;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient"</li>
    </ul>
</asp:Content>
