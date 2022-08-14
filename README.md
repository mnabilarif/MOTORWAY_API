# MOTORWAY_API



https://localhost:44347/api/Account/Register
{
  "Email": "ZeroPoint@motorway.pk",
  "Password": "Pakistan@1947",
  "ConfirmPassword": "Pakistan@1947",
  "Distance" : "0.0"
}


----------------------------
Token
Get
https://localhost:44347/Token
body , text
username=zeropoint@motorway.pk&password=Pakistan@1947&grant_type=password

result
{
    "access_token": "bfm2E1c62Ws33F0qYJPGp86OFdRHwwle5N07U0eYpUzMxhRJ5VoFaWm8ID_cxVx1hDxg64SomCVJmfoJs1RRAa1wU0uKTK6mC2T0mw_mrAnfbF3I4T_H2RLVdfS2YX_Hm22QShx2IDLjChkNP-qchTxrMdqQPtvWHNfc0mZgYAOgsh7vUgUazRiLBXoQySIO31o_uhg9kat0Jg1DWLXIU5L6pHoGFg5jr0G3mOhxcLdNbQzw8aU1u7ymzpTzRIEAbKtpFGuxpukAIyxkbzZFZxDMaym7NDAUpxENj_uT4D4LL9pQL6bFbRsZJixDIpSIsCp7K4xBB0z7Q31uxoPHSbakqDNA9ARxtCThzWbfISws9MhbShNDRPeXAxgU3HU4vHmFzYv1WkaeipW19-DL2nGQWeeG4wNLOzItmJCfXbXk6eZE2CaivwFO6F2VSCg2l6Z5o26QQAO1KCwpoEd_D1NxZ-kLpnSC-e5Z23BMnOXY3aQif5X7IaaZsD72ZhJH",
    "token_type": "bearer",
    "expires_in": 1209599,
    "userName": "ZeroPoint@motorway.pk",
    ".issued": "Sun, 14 Aug 2022 09:13:29 GMT",
    ".expires": "Sun, 28 Aug 2022 09:13:29 GMT"
}

-----------------------------------------------------------------
https://localhost:44347/api/Motorway/AddEntry
use beare token for authorization

body json
{

    "VRN":  "ABD-123",
    "Time" : "2022-08-14 1:59:00.000"
}

https://localhost:44347/api/Motorway/AddExit/
use beare token for authorization

body json
{

    "VRN":  "ABD-123",
    "Time" : "2022-08-14 1:59:00.000" 
}