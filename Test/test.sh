# variables
# baseUrl=http://localhost:5198
baseUrl=http://tzather-test-mock-cs.azurewebsites.net

# create output folder
mkdir -p output

# login
curl --silent --request POST "$baseUrl/identity/login" >output/login.json

# json
curl --silent --request GET "$baseUrl/dashboard/barchart" | jq . >output/barchart.json
curl --silent --request GET "$baseUrl/dashboard/gaugechart" | jq . >output/gaugechart.json

# csv
curl --silent --header "Content-Type:text/csv" --request GET "$baseUrl/dashboard/barchart" >output/barchart.csv
curl --silent --header "Content-Type:text/csv" --request GET "$baseUrl/dashboard/linechart" >output/linechart.csv
curl --silent --header "Content-Type:text/csv" --request GET "$baseUrl/dashboard/piechart" >output/piechart.csv
