using System.Collections;
using System.Globalization;
using System.Linq;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Resources;

namespace SMS.EPA.Site.Controllers.API;

public class ResourcesController : ControllerBase
{

    public string Get()
    {
        var resourceManager = new ResourceManager(typeof(Lang)) { IgnoreCase = true };

        var resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        var dictionary = resourceSet.Cast<DictionaryEntry>();
        var items = dictionary.Select(dict => $"\"{dict.Key}\" : \"{string.Join(",", dict.Value)}\"");
        var jsonString = "{" + string.Join(",", items) + "}";

        return jsonString;
    }
}