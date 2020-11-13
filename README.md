Za pokretanje projekta potrebno je instalirati [Docker Desktop](https://www.docker.com/products/docker-desktop)

Za pokretanje projekta preko docker-compose potrebno je:
   - klonirati repozitorijum u lokalni direktorijum
   - otvoriti Power Shell komandni prozor u korenskom folderu
   - pokrenuti komandu **docker-compose up --build**
   - bildovanje i pokretanje svih projekata prvi put traje duze,
      ali svaki sledeci put je znatno brze
   - nakon toga otvoriti Power Shell prozor u folderu web-app
   - pokrenuti komandu **npm i** kako bi se instalirali node_modules
   - pokrenuti komandu **ng serve**
   - otvoriti localhost:4200 u browser-u

Za pokretanje projekta preko kubernetes potrebno je:
   - u Docker Desktop podesavanjima odabrati **Enable Kubernetes**
   - klonirati repozitorijum u lokalni direktorijum
   - otvoriti Power Shell komandni prozor u korenskom folderu
   - pokrenuti komandu **kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.41.2/deploy/static/provider/cloud/deploy.yaml**
   - pokrenuti komandu **kubectl apply -f k8s** koja ce pokrenuti projekat
   - bildovanje i pokretanje svih projekata prvi put traje duze,
      ali svaki sledeci put je znatno brze
   - nakon toga otvoriti Power Shell prozor u folderu web-app
   - pokrenuti komandu **npm i** kako bi se instalirali node_modules
   - pokrenuti komandu **ng serve**
   - otvoriti localhost:4200 u browser-u

Za login kao admin:
email: admin@gmail.com
password: Admin123!

Za login kao controller:
email: controller@gmail.com
password: Controller123!

Za login kao appuser:
email: appuser@gmail.com
passsword: Appuser123!