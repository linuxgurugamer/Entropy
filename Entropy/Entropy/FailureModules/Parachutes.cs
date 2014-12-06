﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ippo;
using UnityEngine;
using KSP;

namespace coffeeman
{
	public class ModuleParachuteReliability : ippo.FailureModule
	{
		ModuleParachute chute;
		bool canFail;

		public override string DebugName { get { return "Canopy_stock_chutes"; } }
		public override string ScreenName { get { return "Canopy"; } }
		public override string FailureMessage { get { return "The lines of a Parachute have become tangled"; } }
		public override string RepairMessage { get { return "You have repaired Canopy"; } }
		public override string FailGuiName { get { return "Fail Canopy"; } }
		public override string EvaRepairGuiName { get { return "Patch hole"; } }
		public override string MaintenanceString { get { return "Patch Canopy"; } }

		public override bool PartIsActive()
		{
			// A chute is active if its not stowed
			return chute.deploymentState != ModuleParachute.deploymentStates.STOWED;
		}

		protected override void DI_Start(StartState state)
		{
			chute = this.part.Modules.OfType<ModuleParachute>().Single();

			foreach ( Part part_each in this.part.vessel.Parts){ //Make sure that there is at least one other chute on the craft!
				if (part_each != this.part) {
					foreach (PartModule module_each in part_each.Modules) {
						if (module_each is ModuleParachute) {
							this.canFail = true;
						}
					}
				}
			}
		}

		protected override bool DI_FailBegin()
		{
			return canFail;
		}

		protected override void DI_Disable()
		{
			chute.fullyDeployedDrag /= 2;
			chute.semiDeployedDrag /= 2;
		}


		protected override void DI_EvaRepair()
		{ 
			chute.fullyDeployedDrag *= 2;
			chute.semiDeployedDrag *= 2;
		}
	}
}
