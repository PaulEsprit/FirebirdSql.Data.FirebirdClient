﻿/*
 *    The contents of this file are subject to the Initial
 *    Developer's Public License Version 1.0 (the "License");
 *    you may not use this file except in compliance with the
 *    License. You may obtain a copy of the License at
 *    https://github.com/FirebirdSQL/NETProvider/blob/master/license.txt.
 *
 *    Software distributed under the License is distributed on
 *    an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 *    express or implied. See the License for the specific
 *    language governing rights and limitations under the License.
 *
 *    All Rights Reserved.
 */

//$Authors = Jiri Cincura (jiri@cincura.net)

using System;

using FirebirdSql.Data.Common;
using FirebirdSql.Data.FirebirdClient;

namespace FirebirdSql.Data.Services
{
	public sealed class FbValidation2 : FbService
	{

		public string TablesInclude { get; set; }
		public string TablesExclude { get; set; }
		public string IndicesInclude { get; set; }
		public string IndicesExclude { get; set; }
		public int? LockTimeout { get; set; }

		public FbValidation2(string connectionString = null)
			: base(connectionString)
		{ }

		public void Execute()
		{
			EnsureDatabase();

			try
			{
				StartSpb = new ServiceParameterBuffer();
				StartSpb.Append(IscCodes.isc_action_svc_validate);
				StartSpb.Append(IscCodes.isc_spb_dbname, Database);
				if (!string.IsNullOrEmpty(TablesInclude))
					StartSpb.Append(IscCodes.isc_spb_val_tab_incl, TablesInclude);
				if (!string.IsNullOrEmpty(TablesExclude))
					StartSpb.Append(IscCodes.isc_spb_val_tab_excl, TablesExclude);
				if (!string.IsNullOrEmpty(IndicesInclude))
					StartSpb.Append(IscCodes.isc_spb_val_idx_incl, IndicesInclude);
				if (!string.IsNullOrEmpty(IndicesExclude))
					StartSpb.Append(IscCodes.isc_spb_val_idx_excl, IndicesExclude);
				if (LockTimeout.HasValue)
					StartSpb.Append(IscCodes.isc_spb_val_lock_timeout, (int)LockTimeout);

				Open();
				StartTask();
				ProcessServiceOutput();
			}
			catch (Exception ex)
			{
				throw new FbException(ex.Message, ex);
			}
			finally
			{
				Close();
			}
		}
	}
}
