import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class BuildPharmacistItem extends StatelessWidget {
  final String initials;
  final Color avatarBg;
  final TextStyle avatarTextStyle;
  final String name;
  final String specialty;
  final String price;
  const BuildPharmacistItem({
    super.key,
    required this.initials,
    required this.avatarBg,
    required this.avatarTextStyle,
    required this.name,
    required this.specialty,
    required this.price,
  });

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    return Container(
      padding: EdgeInsetsDirectional.all(AppDimensions.spacing12.w),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(18.r),
        boxShadow: [
          BoxShadow(
            color: AppColors.ink.withValues(alpha: 0.03),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Row(
        children: [
          Container(
            width: 44.w,
            height: 44.w,
            decoration: BoxDecoration(color: avatarBg, shape: BoxShape.circle),
            alignment: Alignment.center,
            child: Text(initials, style: avatarTextStyle),
          ),
          SizedBox(width: AppDimensions.spacing12.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Flexible(
                      child: Text(
                        name,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: AppTextStyles.label15InkBold,
                      ),
                    ),
                    SizedBox(width: AppDimensions.spacing4.w),
                    Icon(
                      Icons.verified_rounded,
                      size: 15.sp,
                      color: AppColors.primary,
                    ),
                  ],
                ),
                SizedBox(height: 2.h),
                Text(
                  specialty,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: AppTextStyles.caption12MutedRegular,
                ),
              ],
            ),
          ),
          SizedBox(width: AppDimensions.spacing8.w),
          Column(
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
              RichText(
                text: TextSpan(
                  children: [
                    TextSpan(
                      text: '$price ',
                      style: AppTextStyles.label14InkBold,
                    ),
                    TextSpan(
                      text: s.pharmacistMockupCurrency,
                      style: AppTextStyles.caption12MutedSemiBold,
                    ),
                  ],
                ),
              ),
              SizedBox(height: 3.h),
              Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Container(
                    width: 8.w,
                    height: 8.w,
                    decoration: const BoxDecoration(
                      color: AppColors.availableDot,
                      shape: BoxShape.circle,
                    ),
                  ),
                  SizedBox(width: AppDimensions.spacing4.w),
                  Text(
                    s.pharmacistMockupAvailable,
                    style: AppTextStyles.caption12SuccessSemiBold,
                  ),
                ],
              ),
            ],
          ),
        ],
      ),
    );
  }
}
