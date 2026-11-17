import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/prescription/clinical_summary_card.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/prescription/prescription_details_card.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/prescription/share_summary_toggle_card.dart';

class PrescriptionSummaryMockupCard extends StatelessWidget {
  const PrescriptionSummaryMockupCard({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      padding: EdgeInsetsDirectional.all(AppDimensions.spacing16.w),
      decoration: BoxDecoration(
        color: AppColors.primaryTint,
        borderRadius: BorderRadius.circular(28.r),
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          const PrescriptionDetailsCard(),
          SizedBox(height: AppDimensions.spacing12.h),
          const ClinicalSummaryCard(),
          SizedBox(height: AppDimensions.spacing12.h),
          const ShareSummaryToggleCard(),
        ],
      ),
    );
  }
}