using System.Threading.Tasks;
using Xunit;

namespace NSwag.CodeGeneration.CSharp.Tests
{
    public class OneOfTests
    {
        [Fact]
        public async Task Correct_class_generated_for_oneOf()
        {
            //// Arrange
            var swagger = @"{
  ""openapi"": ""3.0.0"",
            ""info"": {
                ""title"": ""oneOf bug"",
                ""version"": ""1.0""
            },
            ""components"": {
                ""schemas"": {
                    ""request_body"": {
                        ""type"": ""object"",
                        ""additionalProperties"": false,
                        ""oneOf"": [
                        {
                            ""$ref"": ""#/components/schemas/first_id_request""
                        },
                        {
                            ""$ref"": ""#/components/schemas/second_id_request""
                        }
                        ]
                    },
                    ""first_id_request"": {
                        ""type"": ""object"",
                        ""additionalProperties"": false,
                        ""properties"": {
                            ""first_id"": {
                                ""type"": ""string""
                            }
                        }
                    },
                    ""second_id_request"": {
                        ""type"": ""object"",
                        ""additionalProperties"": false,
                        ""properties"": {
                            ""second_id"": {
                                ""type"": ""string""
                            }
                        }
                    }
                }
            }
        }";
            var document = await OpenApiDocument.FromJsonAsync(swagger);

            //// Act
            var settings = new CSharpClientGeneratorSettings { ClassName = "MyClass" };
            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();

            //// Assert
            Assert.Contains(@"public partial class Request_body 
    {
        [Newtonsoft.Json.JsonProperty(""first_id"", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string First_id { get; set; }
    
        [Newtonsoft.Json.JsonProperty(""second_id"", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Second_id { get; set; }
    
    
    }".Replace("\r\n", "\n"), code);
        }

        [Fact]
        public async Task Correct_class_generated_for_oneOf_nullable()
        {
            //// Arrange
            var swagger = @"{
  ""openapi"": ""3.0.0"",
            ""info"": {
                ""title"": ""oneOf bug"",
                ""version"": ""1.0""
            },
            ""components"": {
                ""schemas"": {
                    ""request_body"": {
                        ""type"": ""object"",
                        ""additionalProperties"": false,
                        ""oneOf"": [
                        {
                            ""$ref"": ""#/components/schemas/first_id_request""
                        },
                        {
                            ""$ref"": ""#/components/schemas/second_id_request""
                        }
                        ]
                    },
                    ""first_id_request"": {
                        ""type"": ""object"",
                        ""additionalProperties"": false,
                        ""properties"": {
                            ""first_id"": {
                                ""type"": ""string"",
                                ""nullable"": true                            
                            }
                        }
                    },
                    ""second_id_request"": {
                        ""type"": ""object"",
                        ""additionalProperties"": false,
                        ""properties"": {
                            ""second_id"": {
                                ""type"": ""string"",
                                ""nullable"": true
                            }
                        }
                    }
                }
            }
        }";
            var document = await OpenApiDocument.FromJsonAsync(swagger);

            //// Act
            var settings = new CSharpClientGeneratorSettings { ClassName = "MyClass" };
            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();

            //// Assert
            Assert.Contains(@"public partial class Request_body 
    {
        [Newtonsoft.Json.JsonProperty(""first_id"", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string First_id { get; set; }
    
        [Newtonsoft.Json.JsonProperty(""second_id"", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Second_id { get; set; }
    
    
    }".Replace("\r\n", "\n"), code);
        }
    }
}