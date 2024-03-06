#!/bin/bash
/opt/mssql/bin/sqlservr &
sleep 30
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "U6cFvFW9" -i /init-db.sql
wait
