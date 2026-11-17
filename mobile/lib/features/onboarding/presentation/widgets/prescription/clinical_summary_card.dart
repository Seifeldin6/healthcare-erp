import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class ClinicalSummaryCard extends StatelessWidget {
  const ClinicalSummaryCard({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      width: double.infinity,
      padding: EdgeInsetsDirectional.all(14.w),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(22.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            s.prescriptionMockupClinicalSummary,
            style: AppTextStyles.caption12MutedBoldUppercase,
          ),
          SizedBox(height: 10.h),
          Wrap(
            spacing: AppDimensions.spacing8.w,
            runSpacing: AppDimensions.spacing6.h,
            children: [
              _SummaryChip(
                label: s.prescriptionMockupAllergies,
                bgColor: AppColors.allergyBg,
                textStyle: AppTextStyles.caption12AllergySemiBold,
              ),
              _SummaryChip(
                label: s.prescriptionMockupConditions,
                bgColor: AppColors.conditionBg,
                textStyle: AppTextStyles.caption12ConditionSemiBold,
              ),
              _SummaryChip(
                label: s.prescriptionMockupMedications,
                bgColor: AppColors.primaryTint,
                textStyle: AppTextStyles.caption12PrimarySemiBold,
              ),
            ],
          ),
        ],
      ),
    );
  }
}

class _SummaryChip extends StatelessWidget {
  final String label;
  final Color bgColor;
  final TextStyle textStyle;

  const _SummaryChip({
    required this.label,
    required this.bgColor,
    required this.textStyle,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsetsDirectional.symmetric(
        horizontal: AppDimensions.spacing12.w,
        vertical: AppDimensions.spacing6.h,
      ),
      decoration: BoxDecoration(
        color: bgColor,
        borderRadius: BorderRadius.circular(14.r),
      ),
      child: Text(label, style: textStyle),
    );
  }
}