# create output folder
mkdir -p output

# login
curl --silent --request POST "http://localhost:5198/identity/login" >output/login.json

# json
curl --silent --request GET "http://localhost:5198/dashboard/barchart" | jq . >output/barchart.json
curl --silent --request GET "http://localhost:5198/dashboard/gaugechart" | jq . >output/gaugechart.json

# csv
curl --silent --header "Content-Type:text/csv" --request GET "http://localhost:5198/dashboard/barchart" >output/barchart.csv
curl --silent --header "Content-Type:text/csv" --request GET "http://localhost:5198/dashboard/linechart" >output/linechart.csv
curl --silent --header "Content-Type:text/csv" --request GET "http://localhost:5198/dashboard/piechart" >output/piechart.csv
