#pragma checksum "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "414e2ae31b08b4ec07d08ec84af2b27290a00e90"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Explore), @"mvc.1.0.view", @"/Views/Home/Explore.cshtml")]
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
#nullable restore
#line 1 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
using EstateAgency.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
using EstateAgency.Database;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"414e2ae31b08b4ec07d08ec84af2b27290a00e90", @"/Views/Home/Explore.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a846c84826ae8eb8615d770b486ee13cd1de458b", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Explore : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
  
    Layout = "_Basic";
    ViewData["Title"] = "Explore";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<p>List of Estate objects:</p>\r\n");
#nullable restore
#line 10 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
  
    foreach (var i in DbAdvanced.GetEstateObjects())
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"customcard\">\r\n            <table>\r\n                <tr>\r\n                    <td>Post date</td>\r\n                    <td>");
#nullable restore
#line 17 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
                   Write(i.PostDate.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n                <tr>\r\n                    <td>Price</td>\r\n                    <td>");
#nullable restore
#line 21 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
                   Write(i.Price.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral(" USD</td>\r\n                </tr>\r\n                <tr>\r\n                    <td>Description</td>\r\n                    <td>");
#nullable restore
#line 25 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
                   Write(i.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n            </table>\r\n        </div>\r\n");
#nullable restore
#line 29 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\Explore.cshtml"
    }

#line default
#line hidden
#nullable disable
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
