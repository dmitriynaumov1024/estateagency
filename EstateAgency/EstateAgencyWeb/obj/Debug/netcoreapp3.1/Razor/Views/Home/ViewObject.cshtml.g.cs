#pragma checksum "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "11540acfd45d1e062f35c67e38e9e40dc8775053"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ViewObject), @"mvc.1.0.view", @"/Views/Home/ViewObject.cshtml")]
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
#line 1 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
using EstateAgency.Entities;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"11540acfd45d1e062f35c67e38e9e40dc8775053", @"/Views/Home/ViewObject.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a846c84826ae8eb8615d770b486ee13cd1de458b", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ViewObject : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
  
    Layout = "_Basic"; 
    ViewData["Title"] = "Object";
    ViewData["script"] = "site-viewobject.js";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<p>Інформація про об\'єкт</p>\r\n<div class=\"customcard\">\r\n    <table class=\"objectdetail\">\r\n");
            WriteLiteral("        <tr>\r\n            <td>Ціна</td>\r\n            <td><b>");
#nullable restore
#line 14 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
              Write(Model.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b> USD</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Продавець</td>\r\n            <td><b>");
#nullable restore
#line 18 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
              Write(ViewData["Seller"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </b></td>\r\n        </tr>\r\n        <tr>\r\n            <td>Опубліковано</td>\r\n            <td>");
#nullable restore
#line 22 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
            Write(((DateTime)Model.PostDate).ToLocalTime());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n\r\n");
#nullable restore
#line 26 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
         if (Model.Variant == (byte)'h')
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>Варіант</td>\r\n                <td>Будинок</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Площа земельної ділянки</td>\r\n                <td>");
#nullable restore
#line 34 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.LandArea);

#line default
#line hidden
#nullable disable
            WriteLiteral(" а </td>\r\n            </tr>\r\n            <tr>\r\n                <td>Житлова площа</td>\r\n                <td>");
#nullable restore
#line 38 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.HomeArea);

#line default
#line hidden
#nullable disable
            WriteLiteral(" кв.м. </td>\r\n            </tr>\r\n            <tr>\r\n                <td>Ціна 1 кв.м.</td>\r\n                <td>");
#nullable restore
#line 42 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                Write((int)(Model.Price / Model.HomeArea));

#line default
#line hidden
#nullable disable
            WriteLiteral(" USD </td>\r\n            </tr>\r\n            <tr>\r\n                <td>Кількість поверхів</td>\r\n                <td>");
#nullable restore
#line 46 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.FloorCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Кількість кімнат</td>\r\n                <td>");
#nullable restore
#line 50 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.RoomCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n");
#nullable restore
#line 52 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
        }

        

#line default
#line hidden
#nullable disable
#nullable restore
#line 55 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
         if (Model.Variant == (byte)'f')
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>Варіант</td>\r\n                <td>Квартира</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Номер поверху</td>\r\n                <td>");
#nullable restore
#line 63 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.Floor);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Житлова площа</td>\r\n                <td>");
#nullable restore
#line 67 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.HomeArea);

#line default
#line hidden
#nullable disable
            WriteLiteral(" кв.м.</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Ціна 1 кв.м.</td>\r\n                <td>");
#nullable restore
#line 71 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                Write((int)(Model.Price / Model.HomeArea));

#line default
#line hidden
#nullable disable
            WriteLiteral(" USD </td>\r\n            </tr>\r\n            <tr>\r\n                <td>К-ть кімнат</td>\r\n                <td>");
#nullable restore
#line 75 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.RoomCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n");
#nullable restore
#line 77 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
        }

        

#line default
#line hidden
#nullable disable
#nullable restore
#line 80 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
         if (Model.Variant == (byte)'l')
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>Варіант</td>\r\n                <td>Земельна ділянка</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Площа</td>\r\n                <td>");
#nullable restore
#line 88 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
               Write(Model.LandArea);

#line default
#line hidden
#nullable disable
            WriteLiteral(" a</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Ціна 1 а землі</td>\r\n                <td>");
#nullable restore
#line 92 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                Write((int)(Model.Price / Model.LandArea));

#line default
#line hidden
#nullable disable
            WriteLiteral(" USD </td>\r\n            </tr>\r\n");
#nullable restore
#line 94 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 96 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
         if (Model.Variant == (byte)'o')
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>Варіант</td>\r\n                <td>інше</td>\r\n            </tr>\r\n");
#nullable restore
#line 102 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        <tr>\r\n            <td>Опис</td>\r\n            <td>");
#nullable restore
#line 106 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
           Write(Model.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Теги</td>\r\n            <td>\r\n");
#nullable restore
#line 111 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                 if ((Model as EstateObject).Tags != null)
                {
                    foreach (string i in (Model as EstateObject).Tags)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<span>");
#nullable restore
#line 114 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                      Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span><br />");
#nullable restore
#line 114 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                                          }
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </td>\r\n        </tr>\r\n        <tr>\r\n            <td>URL фотографій</td>\r\n            <td>\r\n");
#nullable restore
#line 121 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                 if ((Model as EstateObject).PhotoUrls != null)
                {
                    foreach (string i in (Model as EstateObject).PhotoUrls)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<span>");
#nullable restore
#line 124 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                      Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span><br />");
#nullable restore
#line 124 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                                          }
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </td>\r\n        </tr>\r\n        <tr>\r\n            <td>Стан [0..5]</td>\r\n            <td>");
#nullable restore
#line 130 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
           Write(Model.State);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Локація</td>\r\n            <td>");
#nullable restore
#line 134 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
           Write(ViewData["LocationFull"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Адреса:</td>\r\n            <td>");
#nullable restore
#line 138 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
            Write(Model.StreetName + ", буд. " + Model.HouseNumber + ((Model as EstateObject).Variant==(byte)'f'? (", кв. " + Model.FlatNumber.ToString()):""));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n\r\n");
#nullable restore
#line 142 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
         if (ViewData["isBookmarked"] != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td colspan=\"2\">\r\n                    <button id=\"bookmark\"></button>\r\n                    <script>\r\n                    var currentObjectBookmark = ");
#nullable restore
#line 148 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                                            Write((bool)ViewData["isBookmarked"]?"true":"false");

#line default
#line hidden
#nullable disable
            WriteLiteral(";\r\n                    var objectID = ");
#nullable restore
#line 149 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                              Write(ViewData["ObjectID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(";\r\n                    </script>\r\n                </td>\r\n            </tr>\r\n");
#nullable restore
#line 153 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </table>\r\n");
#nullable restore
#line 155 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
     foreach (string i in (Model as EstateObject).PhotoUrls)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<img");
            BeginWriteAttribute("src", " src=\"", 4571, "\"", 4584, 2);
            WriteAttributeValue("", 4577, "/img/", 4577, 5, true);
#nullable restore
#line 156 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
WriteAttributeValue("", 4582, i, 4582, 2, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />");
#nullable restore
#line 156 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\ViewObject.cshtml"
                          }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
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
