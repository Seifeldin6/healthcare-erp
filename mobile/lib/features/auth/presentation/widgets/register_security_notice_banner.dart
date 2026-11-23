import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterSecurityNoticeBanner extends StatelessWidget {
  const RegisterSecurityNoticeBanner({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      padding: EdgeInsets.all(12.w),
      decoration: BoxDecoration(
        color: AppColors.primary.withValues(alpha: 0.08),
        borderRadius: BorderRadius.circular(12.r),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(
            Icons.verified_user_outlined,
            color: AppColors.primary,
            size: 18.sp,
          ),
          SizedBox(width: 10.w),
          Expanded(
            child: Text.rich(
              TextSpan(
                style: AppTextStyles.caption12MutedRegular.copyWith(
                  color: AppColors.primary,
                ),
                children: [
                  TextSpan(text: s.registerInfoBannerPrefix),
                  TextSpan(
                    text: s.createAccountButton,
                    style: AppTextStyles.caption12MutedRegular.copyWith(
                      color: AppColors.primary,
                      fontWeight: FontWeight.w700,
                    ),
                  ),
                  TextSpan(text: s.registerInfoBannerSuffix),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
