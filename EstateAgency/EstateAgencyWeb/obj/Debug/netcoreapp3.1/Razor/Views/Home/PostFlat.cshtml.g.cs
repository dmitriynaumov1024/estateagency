#pragma checksum "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\PostFlat.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "38016ecc7a584a9902a59286c6a8ba407821e62c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_PostFlat), @"mvc.1.0.view", @"/Views/Home/PostFlat.cshtml")]
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
#line 1 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\PostFlat.cshtml"
using EstateAgency.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\PostFlat.cshtml"
using EstateAgency.Database;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"38016ecc7a584a9902a59286c6a8ba407821e62c", @"/Views/Home/PostFlat.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a846c84826ae8eb8615d770b486ee13cd1de458b", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_PostFlat : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Post_Stage2", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("customcard container20em"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\PostFlat.cshtml"
  
    Layout = "_Basic";
    ViewData["Title"] = "New flat";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2>New house</h2>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "38016ecc7a584a9902a59286c6a8ba407821e62c5215", async() => {
                WriteLiteral("\r\n    <input type=\"hidden\" name=\"location\"");
                BeginWriteAttribute("value", " value=\"", 304, "\"", 333, 1);
#nullable restore
#line 11 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\PostFlat.cshtml"
WriteAttributeValue("", 312, ViewData["location"], 312, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"variant\"");
                BeginWriteAttribute("value", " value=\"", 378, "\"", 406, 1);
#nullable restore
#line 12 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\PostFlat.cshtml"
WriteAttributeValue("", 386, ViewData["variant"], 386, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />

    <label>?????????? ????????????:</label>
    <input name=""streetname"" type=""text"" maxlength=""64"" placeholder=""??????????????: ???????????????? ????????????????"" />

    <label>?????????? ??????????????:</label>
    <input name=""housenumber"" type=""text"" maxlength=""5"" style=""width: 8em;"" />

    <label>?????????? ????????????????:</label>
    <input name=""flatnumber"" type=""text"" maxlength=""4"" style=""width: 8em;"" />

    <label>?????????? ????????????????, ????.??:</label>
    <input name=""homearea"" type=""text"" maxlength=""7"" style=""width: 8em;"" />

    <label>????????????:</label>
    <input name=""floor"" type=""text"" maxlength=""3"" style=""width: 8em;"" />

    <label>?????????????????? ????????????:</label>
    <input name=""roomcount"" type=""text"" maxlength=""2"" style=""width: 8em;"" />

    <label>???????? ????'????????:</label>
    <textarea name=""description"" style=""resize:none; height: 10em;""></textarea>

    <label>???????? [0..5]:</label>
    <input name=""state"" type=""text"" maxlength=""1"" style=""width: 8em;"" />

    <label>????????, USD:</label>
    <input type=""text"" maxlength=""7"" name=""pri");
                WriteLiteral(@"ce"" style=""width: 8em;"" />

    <label>????????:</label>
    <textarea name=""tags"" placeholder=""Enter tags here, ex.: water, gas, electricity"" style=""resize:none;""></textarea>

    <label>????????:</label>
    <input name=""photos"" type=""file"" accept=""image/*"" multiple required />
    <br />
    <p style=""color:darkred; font-weight:bold;"">");
#nullable restore
#line 47 "E:\Proj_1\estateagency\EstateAgency\EstateAgencyWeb\Views\Home\PostFlat.cshtml"
                                           Write(ViewData["ErrorMessage"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" &ensp;</p>\r\n    <br />\r\n    <button type=\"submit\">??????????????????</button>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
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
