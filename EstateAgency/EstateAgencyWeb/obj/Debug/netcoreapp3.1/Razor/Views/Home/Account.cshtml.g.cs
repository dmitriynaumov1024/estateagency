#pragma checksum "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Account.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "09275c477af30beb43ca227bb59c4e65536cd685"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Account), @"mvc.1.0.view", @"/Views/Home/Account.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\_ViewImports.cshtml"
using EstateAgencyWeb;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\_ViewImports.cshtml"
using EstateAgencyWeb.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09275c477af30beb43ca227bb59c4e65536cd685", @"/Views/Home/Account.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a846c84826ae8eb8615d770b486ee13cd1de458b", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Account : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Account.cshtml"
  
    ViewData["Title"]="Account";
    Layout="_Basic";

#line default
#line hidden
#nullable disable
            WriteLiteral("<h2>Мій обліковий запис</h2>\r\n<div class=\"customcard container60\">\r\n    <table class=\"objectdetail\" style=\"word-break:normal;\">\r\n        <tr>\r\n            <td>Прізвище</td>\r\n            <td>");
#nullable restore
#line 10 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Account.cshtml"
           Write(Model.Surname);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Ім\'я</td>\r\n            <td>");
#nullable restore
#line 14 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Account.cshtml"
           Write(Model.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Телефон</td>\r\n            <td>");
#nullable restore
#line 18 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Account.cshtml"
           Write(Model.Phone);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n        <tr>\r\n            <td>E-mail</td>\r\n            <td>");
#nullable restore
#line 22 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Account.cshtml"
           Write(Model.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Адреса </td>\r\n            <td>");
#nullable restore
#line 26 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Account.cshtml"
           Write(ViewData["Location"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n    </table>\r\n    <br />\r\n    <a href=\"/Logout\">&emsp; Вийти</a>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
