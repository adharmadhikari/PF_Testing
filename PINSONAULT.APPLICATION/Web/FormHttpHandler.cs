using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PathfinderModel;
using System.IO;
using System.Configuration;

namespace Pinsonault.Web
{
    /// <summary>
    /// Summary description for FormHttpHandler
    /// </summary>
    public abstract class FormHttpHandler : GenericHttpHandler
    {
        public FormHttpHandler()
        {
        }

        public override bool IsReusable
        {
            get { return false; }
        }

        public virtual bool ConfirmClientDrug
        {
            get { return true; }
        }

        /// <summary>
        /// returns the user role required to dowload the form
        /// </summary>
        protected abstract string SecurityRole { get; }

        /// <summary>
        /// Returns the folder where forms are located - if the full path comes from the database return an empty string.
        /// </summary>
        protected abstract string FormFolder { get; }

        /// <summary>
        /// Returns the form file name from the database.  If only form's file name is returned FormFolder property must return the path where it is located.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="planID"></param>
        /// <param name="segmentID"></param>
        /// <param name="drugID"></param>
        /// <param name="theraID"></param>
        /// <returns></returns>
        protected abstract string GetFormName(PathfinderEntities ctx, int planID, int segmentID, int drugID, int theraID);
        

        protected override void InternalProcessRequest(System.Web.HttpContext context)
        {
            string folder = FormFolder;
            if ( string.IsNullOrEmpty(folder) )
                throw new HttpException(500, "Form folder has not been specified in web.config.");


            int planID;
            int theraID;
            int drugID;
            int segmentID;

            int.TryParse(context.Request["planID"], out planID);
            int.TryParse(context.Request["theraID"], out theraID);
            int.TryParse(context.Request["drugID"], out drugID);
            int.TryParse(context.Request["segmentID"], out segmentID);

            using ( PathfinderEntities ctx = new PathfinderEntities() )
            {
                //MedPolicyForm form = (from mf in ctx.MedPolicyFormSet
                //        where
                //        mf.Plan_ID == planID
                //        && (mf.Section_ID == segmentID)
                //        && (mf.Thera_ID == null || mf.Thera_ID == theraID)
                //        && (mf.Drug_ID == null || mf.Drug_ID == drugID)
                //        orderby mf.Form_Rank
                //        select mf).FirstOrDefault();

                //Before requesting form make sure the user has access to that form type and that they purchased the requested drug
                if ( context.User.IsInRole(SecurityRole) && (!ConfirmClientDrug || ctx.GetUserDrugList(Pinsonault.Web.Session.ClientID).Where(d => d.ID == drugID).Count() > 0) )
                {
                    string form = GetFormName(ctx, planID, segmentID, drugID, theraID);
                    if ( !string.IsNullOrEmpty(form) )
                    {
                        string path = Path.Combine(folder, form);
                        if ( File.Exists(path) )
                        {
                            context.Response.ContentType = "application/pdf";
                            context.Response.TransmitFile(path);
                            return;
                        }
                    }

                    throw new HttpException(404, "Form was not found.");
                }

                throw new HttpException(403, "You are not authorized to access the requested form.");
            }            
        }
    }

    public class PDLFormHttpHandler : FormHttpHandler
    {
        public override bool ConfirmClientDrug
        {
            get { return false; }
        }

        protected override string SecurityRole { get { return "ta"; } }

        protected override string FormFolder
        {
            get { return Pinsonault.Web.Support.GetFormsFolder(@"PDL"); }
        }

        protected override string GetFormName(PathfinderEntities ctx, int planID, int segmentID, int drugID, int theraID)
        {
            return (from p in ctx.PlanInfoStateMedicaidSet where p.Plan_ID == planID select p.SM_Preferred_Drug_List).FirstOrDefault();            
        }
    }

    public class PAFormHttpHandler : FormHttpHandler
    {
        protected override string SecurityRole
        {
            get { return "paforms"; }
        }

        protected override string FormFolder
        {
            get { return Pinsonault.Web.Support.GetFormsFolder(@"PA"); }
        }

        protected override string GetFormName(PathfinderEntities ctx, int planID, int segmentID, int drugID, int theraID)
        {
            //orders by drug id so if null (0) drug id is 2nd in list and we only get first
            PlanForm form = (from f in ctx.PlanFormSet
                             where
                             f.Plan_ID == planID
                             && f.Segment_ID == segmentID
                             && (f.Drug_ID == drugID || f.Drug_ID == 0)
                             orderby f.Drug_ID descending
                             select f).FirstOrDefault();

            if ( form != null )
                return form.PA_Form;

            return null;
        }
    }

    public class MedPolicyFormHttpHandler : FormHttpHandler
    {
        protected override string SecurityRole
        {
            get { return "medforms"; }
        }

        protected override string FormFolder
        {
            get { return Pinsonault.Web.Support.GetFormsFolder(@"Med_Policy"); }
        }

        protected override string GetFormName(PathfinderEntities ctx, int planID, int segmentID, int drugID, int theraID)
        {
            //orders by drug id so if null (0) drug id is 2nd in list and we only get first
            PlanForm form = (from f in ctx.PlanFormSet
                             where
                             f.Plan_ID == planID
                             && f.Segment_ID == segmentID
                             && (f.Drug_ID == drugID || f.Drug_ID == 0)
                             orderby f.Drug_ID descending
                             select f).FirstOrDefault();

            if ( form != null )
                return form.Med_Policy_Form;

            return null;
        }
    }
}