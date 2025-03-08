# MagicOnionSample
This is a sample project for MagicOnion.
Personal project for learning MagicOnion.

# Environment
- Unity 2022.3.12f1
- MagicOnion 7.0.2
- .NET 8.0

# How to use
## Local
1. Open solution with Visual Studio( or Rider)
2. Run MagicOnion.Server
3. Open Unity project (src/MyApp.Unity)
4. Play the scene (SampleScene)

if you want to test multiple clients, you can clone the Unity project with ParrelSync.( https://github.com/VeriorPies/ParrelSync )

## Docker (LocalPC)
cd ./src
docker-compose up --build

then you can access the server with `http://localhost:12345` (http2 client need!)

## Docker (AWS)
1. Create EC2 instance
2. Install Docker
3. Clone this repository
4. Run `docker-compose up --build`
5. Access the server with `http://<EC2 public IP>:12345` (http2 client need!)

todo: https-portal and reverse proxy

# Contribution
1. Fork this repository
2. Create a new branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request
6. Wait for review
7. Merge if approved
8. Done!

# License
This library is under the MIT License.