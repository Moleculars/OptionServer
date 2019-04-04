
# set server address
.\option-SetServer.ps1 -uri "https://localhost:5001"

# create an user account
.\option-AddUser.ps1 -login test -password toto -Pseudo blackman -Mail gaelgael@yopmail.com

# authenticate on server
.\option-connect.ps1 -login test -password toto


#create an account
.\option-AddAccount.ps1 -accountName parcelAccount2

# list all accounts tht youar cane access
.\option-ListAccounts.ps1