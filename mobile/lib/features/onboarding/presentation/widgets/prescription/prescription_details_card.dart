import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:healthcare_app/core/helpers/constants.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class PrescriptionDetailsCard extends StatelessWidget {
  const PrescriptionDetailsCard({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      padding: EdgeInsetsDirectional.all(14.w),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(22.r),
      ),
      child: Column(
        children: [
          Row(
            children: [
              SvgPicture.asset(
                AppAssets.herbalLeafIcon,
                width: 40.w,
                height: 40.w,
              ),
              SizedBox(width: AppDimensions.spacing12.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      s.prescriptionMockupTitle,
                      style: AppTextStyles.label15InkBold,
                    ),
                    SizedBox(height: 2.h),
                    Text(
                      s.prescriptionMockupCode,
                      style: AppTextStyles.caption12MutedRegular,
                    ),
                  ],
                ),
              ),
              Container(
                height: 26.h,
                padding: EdgeInsetsDirectional.symmetric(
                  horizontal: AppDimensions.spacing12.w,
                ),
                decoration: BoxDecoration(
                  color: AppColors.primaryTint,
                  borderRadius: BorderRadius.circular(13.r),
                ),
                alignment: Alignment.center,
                child: Text(
                  s.prescriptionMockupPdf,
                  style: AppTextStyles.caption12PrimaryBold,
                ),
              ),
            ],
          ),
          Padding(
            padding: EdgeInsetsDirectional.symmetric(vertical: 10.h),
            child: const Divider(
              height: 1,
              thickness: 1,
              color: AppColors.line,
            ),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                s.prescriptionMockupHerb1,
                style: AppTextStyles.caption13InkSemiBold,
              ),
              Text(
                s.prescriptionMockupDose1,
                style: AppTextStyles.caption12MutedRegular,
              ),
            ],
          ),
          SizedBox(height: AppDimensions.spacing6.h),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                s.prescriptionMockupHerb2,
                style: AppTextStyles.caption13InkSemiBold,
              ),
              Text(
                s.prescriptionMockupDose2,
                style: AppTextStyles.caption13MutedRegular,
              ),
            ],
          ),
        ],
      ),
    );
  }
}
