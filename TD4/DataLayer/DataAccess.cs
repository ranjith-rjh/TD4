using Npgsql;
using System;
using System.Data;


namespace DataLayer
{
    public class DataAccess // A MODIFIER SI VOTRE PROJET A UN AUTRE NOM
    {
        public NpgsqlConnection? NpgSQLConnect { get; set; }

        /// <summary>
        /// Connexion à la base de données
        /// </summary>
        /// <returns> Retourne un booléen indiquant si la connexion est ouverte ou fermée</returns>
        private bool OpenConnection()
        {
            try
            {
                NpgSQLConnect = new NpgsqlConnection
                {
                    // A MODIFIER SI VOTRE BD A UN AUTRE NOM

                    // ------- CHEZ MOI -------
                    ConnectionString = "Server=localhost;" +
                                        "port=5433;" +
                                        "Database=BDComptesBancaires;" +
                                        "uid=postgres;" +
                                        "password=RanJitH007;"

                    // ------- CHEZ IUT -------
                    //ConnectionString = "Server=localhost;" +
                    //                    "port=5432;" +
                    //                    "Database=BDComptesBancaires;" +
                    //                    "uid=postgres;" +
                    //                    "password=postgres;"

                    // ------- SERVEUR DISTANT -------
                    //ConnectionString = "Server=srv-peda-new;" +
                    //                    "port=5433;" +
                    //                    "Database=bdcomptesbancairesappassar;" +
                    //                    "uid=appassar;" +
                    //                    "password=Oi67gS;"
                };
                NpgSQLConnect.Open();
                return NpgSQLConnect.State.Equals(System.Data.ConnectionState.Open);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Déconnexion de la base de données
        /// </summary>
        private void CloseConnection()
        {
            if (NpgSQLConnect!=null)
                if (NpgSQLConnect.State.Equals(System.Data.ConnectionState.Open))
                {
                    NpgSQLConnect.Close();
                }
        }

        /// <summary>
        /// Accès à des données en lecture
        /// </summary>
        /// <param name="getQuery">Requête SELECT de sélection de données</param>
        /// <returns>Retourne un DataTable contenant les lignes renvoyées par le SELECT</returns>
        public DataTable? GetData(string getQuery)
        {
            try
            {
                if (OpenConnection())
                {
                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(getQuery, NpgSQLConnect);
                    NpgsqlDataAdapter npgsqlAdapter = new NpgsqlDataAdapter
                    {
                        SelectCommand = npgsqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    npgsqlAdapter.Fill(dataTable);
                    CloseConnection();
                    return dataTable;
                }
                else
                    return null;
            }
            catch
            {
                CloseConnection();
                return null;
            }
        }

        /// <summary>
        /// Permet d'insérer, supprimer ou modifier des données
        /// </summary>
        /// <param name="setQuery">Requête SQL permettant d'insérer (INSERT), supprimer (DELETE) ou modifier des données (UPDATE)</param>
        /// <returns>Retourne un entier contenant le nombre de lignes ajoutées/supprimées/modifiées</returns>
        public int SetData(string setQuery)
        {
            try
            {
                if (OpenConnection())
                {
                    NpgsqlCommand sqlCommand = new NpgsqlCommand(setQuery, NpgSQLConnect);
                    int modifiedLines = sqlCommand.ExecuteNonQuery();
                    CloseConnection();
                    return modifiedLines;
                }
                else
                    return 0;
            }
            catch
            {
                CloseConnection();
                return 0;
            }
        }

    }
}
