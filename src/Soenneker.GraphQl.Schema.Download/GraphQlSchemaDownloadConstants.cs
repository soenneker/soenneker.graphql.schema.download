namespace Soenneker.GraphQl.Schema.Download;

internal static class GraphQlSchemaDownloadConstants
{
    internal const string IntrospectionQuery = """
                                               query IntrospectionQuery {
                                                 __schema {
                                                   queryType {
                                                     name
                                                   }
                                                   mutationType {
                                                     name
                                                   }
                                                   subscriptionType {
                                                     name
                                                   }
                                                   types {
                                                     ...FullType
                                                   }
                                                   directives {
                                                     name
                                                     description
                                                     isRepeatable
                                                     locations
                                                     args(includeDeprecated: true) {
                                                       ...InputValue
                                                     }
                                                   }
                                                 }
                                               }

                                               fragment FullType on __Type {
                                                 kind
                                                 name
                                                 description
                                                 specifiedByURL
                                                 fields(includeDeprecated: true) {
                                                   name
                                                   description
                                                   args(includeDeprecated: true) {
                                                     ...InputValue
                                                   }
                                                   type {
                                                     ...TypeRef
                                                   }
                                                   isDeprecated
                                                   deprecationReason
                                                 }
                                                 inputFields(includeDeprecated: true) {
                                                   ...InputValue
                                                 }
                                                 interfaces {
                                                   ...TypeRef
                                                 }
                                                 enumValues(includeDeprecated: true) {
                                                   name
                                                   description
                                                   isDeprecated
                                                   deprecationReason
                                                 }
                                                 possibleTypes {
                                                   ...TypeRef
                                                 }
                                               }

                                               fragment InputValue on __InputValue {
                                                 name
                                                 description
                                                 type {
                                                   ...TypeRef
                                                 }
                                                 defaultValue
                                                 isDeprecated
                                                 deprecationReason
                                               }

                                               fragment TypeRef on __Type {
                                                 kind
                                                 name
                                                 ofType {
                                                   kind
                                                   name
                                                   ofType {
                                                     kind
                                                     name
                                                     ofType {
                                                       kind
                                                       name
                                                       ofType {
                                                         kind
                                                         name
                                                         ofType {
                                                           kind
                                                           name
                                                           ofType {
                                                             kind
                                                             name
                                                             ofType {
                                                               kind
                                                               name
                                                             }
                                                           }
                                                         }
                                                       }
                                                     }
                                                   }
                                                 }
                                               }
                                               """;
}