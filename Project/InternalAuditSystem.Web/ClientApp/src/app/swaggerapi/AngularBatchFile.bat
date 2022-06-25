@ECHO OFF

set executable="../swaggerapi/swagger-codegen-cli-3.0.19.jar"

set ags=generate -i http://localhost:5000/swagger/v1/swagger.json -l typescript-angular -o "../swaggerapi/AngularFiles"

 java -jar %executable% %ags%
