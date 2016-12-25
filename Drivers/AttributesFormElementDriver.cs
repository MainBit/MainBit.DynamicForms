using System.Collections.Generic;
using Orchard.DisplayManagement;
using Orchard.DynamicForms.Elements;
using Orchard.Forms.Services;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;
using Orchard.Layouts.Services;
using Orchard.Tokens;
using DescribeContext = Orchard.Forms.Services.DescribeContext;
using Newtonsoft.Json;
using Orchard.Layouts.Helpers;

namespace MainBit.DynamicForms.Drivers {
    public class AttributesFormElementDriver : FormsElementDriver<FormElement>
    {
        private readonly ITokenizer _tokenizer;

        public AttributesFormElementDriver(
            IFormsBasedElementServices formsServices,
            ITokenizer tokenizer,
            IShapeFactory shapeFactory
            ) : base(formsServices) {
            _tokenizer = tokenizer;
            New = shapeFactory;
        }

        public override int Priority {
            get { return 500; }
        }

        protected override IEnumerable<string> FormNames {
            get { yield return "AttributesFormElement"; }
        }

        public dynamic New { get; set; }

        protected override void DescribeForm(DescribeContext context) {
            context.Form("AttributesFormElement", factory => {
                var shape = (dynamic)factory;
                var form = shape.Fieldset(
                    Id: "AttributesFormElement",
                    _Span: shape.Textbox(
                        Id: "Attributes",
                        Name: "Attributes",
                        Title: "Custom attributes",
                        Classes: new[] { "text", "large", "tokenized" },
                        Description: T("The attributes of this form field in json format. Ex. { \"name1\": \"value1\", \"name2\": 123 }")));

                return form;
            });
        }

        protected override void OnDisplaying(FormElement element, ElementDisplayingContext context)
        {
            IDictionary<string, string> attributes;
            try
            {
                attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(element.Data["Attributes"]);
            }
            catch
            {
                return;
            }

            foreach (var attribute in attributes)
            {
                // http://mdameer.com/orchard-cms/how-to-add-placeholder-to-text-field-in-dynamic-forms
                // ClientValidationAttributes because of bug (Attributes property should be using instead later)
                context.ElementShape.ClientValidationAttributes.Add(
                    attribute.Key,
                    _tokenizer.Replace(attribute.Value, context.GetTokenData()));
            };
        }
    }
}